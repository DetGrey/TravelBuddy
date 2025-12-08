using Neo4j.Driver;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;

// CLASS
public class Neo4jTripRepository : ITripRepository
{
    private readonly IDriver _driver;

    public Neo4jTripRepository(IDriver driver)
    {
        _driver = driver;
    }

    // --------------------------------------------------------------------
    // Helpers
    // --------------------------------------------------------------------

    // Parse various Neo4j date representations into DateOnly
    private static DateOnly ParseIsoDate(object value)
    {
        if (value == null)
            throw new InvalidOperationException("Null date value from Neo4j");

        switch (value)
        {
            case LocalDate ld:
                return new DateOnly(ld.Year, ld.Month, ld.Day);

            case DateTime dt:
                return DateOnly.FromDateTime(dt);

            case string s:
                // Expecting ISO date like "2024-01-31"
                return DateOnly.Parse(s);

            default:
                return DateOnly.Parse(value.ToString()!);
        }
    }

    // Helper: generate next BuddyId (global over all BUDDY_ON relationships)
    private async Task<int> GetNextBuddyIdAsync()
    {
        await using var session = _driver.AsyncSession();
        var cursor = await session.RunAsync(@"
            MATCH ()-[b:BUDDY_ON]->()
            RETURN coalesce(max(b.buddyId), 0) AS MaxId
        ");
        var record = await cursor.SingleAsync();
        var maxId = record["MaxId"].As<int>();
        return maxId + 1;
    }

    // --------------------------------------------------------------------
    // Search available trips / destinations
    // --------------------------------------------------------------------
    public async Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
        DateOnly? reqStart,
        DateOnly? reqEnd,
        string? country,
        string? state,
        string? name,
        int? partySize,
        string? q)
    {
        await using var session = _driver.AsyncSession();

        var cypher = @"
            MATCH (t:Trip)-[hs:HAS_STOP]->(d:Destination)
            OPTIONAL MATCH (u:User)-[b:BUDDY_ON { tripDestinationId: hs.tripDestinationId }]->(t)
            WITH t, hs, d,
                sum(
                    CASE WHEN toLower(b.requestStatus) = 'accepted'
                        THEN coalesce(b.personCount, 1)
                        ELSE 0
                    END
                ) AS acceptedPersons
            WITH t, hs, d, acceptedPersons,
                CASE
                WHEN t.maxBuddies IS NULL THEN 0
                WHEN t.maxBuddies - acceptedPersons < 0 THEN 0
                ELSE t.maxBuddies - acceptedPersons
                END AS remaining
            WHERE
            ($reqStart IS NULL OR date(hs.endDate) >= date($reqStart)) AND
            ($reqEnd   IS NULL OR date(hs.startDate) <= date($reqEnd)) AND
            ($country  IS NULL OR toLower(d.country) = toLower($country)) AND
            ($state    IS NULL OR toLower(coalesce(d.state, '')) = toLower($state)) AND
            ($name     IS NULL OR toLower(d.name) CONTAINS toLower($name)) AND
            ($q        IS NULL OR toLower(
                                    d.name + ' ' +
                                    d.country + ' ' +
                                    coalesce(d.state, '') + ' ' +
                                    coalesce(t.description, '') + ' ' +
                                    coalesce(t.tripName, '')
                                ) CONTAINS toLower($q)) AND
            ($partySize IS NULL OR remaining >= $partySize)
            RETURN
            hs.tripDestinationId AS TripDestinationId,
            t.tripId             AS TripId,
            d.destinationId      AS DestinationId,
            d.name               AS DestinationName,
            d.country            AS Country,
            coalesce(d.state, '') AS State,
            hs.startDate         AS DestinationStart,
            hs.endDate           AS DestinationEnd,
            coalesce(t.maxBuddies, 0) AS MaxBuddies,
            acceptedPersons      AS AcceptedPersons,
            remaining            AS RemainingCapacity
            ";

        var cursor = await session.RunAsync(cypher, new
        {
            reqStart = reqStart?.ToString("yyyy-MM-dd"),
            reqEnd   = reqEnd?.ToString("yyyy-MM-dd"),
            country,
            state,
            name,
            partySize,
            q
        });

        var records = await cursor.ToListAsync();
        var results = new List<TripDestinationSearchResult>();

        foreach (var record in records)
        {
            results.Add(new TripDestinationSearchResult
            {
                TripDestinationId = record["TripDestinationId"].As<int>(),
                TripId            = record["TripId"].As<int>(),
                DestinationId     = record["DestinationId"].As<int>(),
                DestinationName   = record["DestinationName"].As<string>(),
                Country           = record["Country"].As<string>(),
                State             = record["State"].As<string>(),
                DestinationStart  = ParseIsoDate(record["DestinationStart"]),
                DestinationEnd    = ParseIsoDate(record["DestinationEnd"]),
                MaxBuddies        = record["MaxBuddies"].As<int>(),
                AcceptedPersons   = record["AcceptedPersons"].As<int>(),
                RemainingCapacity = record["RemainingCapacity"].As<int>()
            });
        }

        return results;
    }

    // --------------------------------------------------------------------
    // Get all destinations
    // --------------------------------------------------------------------
    public async Task<IEnumerable<Destination>> GetDestinationsAsync()
    {
        await using var session = _driver.AsyncSession();

        var cypher = @"
            MATCH (d:Destination)
            RETURN
            d.destinationId AS DestinationId,
            d.name          AS Name,
            d.state         AS State,
            d.country       AS Country,
            d.longitude     AS Longitude,
            d.latitude      AS Latitude
            ORDER BY d.country, coalesce(d.state, ''), d.name
            ";

        var cursor = await session.RunAsync(cypher);
        var records = await cursor.ToListAsync();

        var list = new List<Destination>();

        foreach (var record in records)
        {
            double? lon = record["Longitude"].As<double?>();
            double? lat = record["Latitude"].As<double?>();

            list.Add(new Destination
            {
                DestinationId = record["DestinationId"].As<int>(),
                Name          = record["Name"].As<string>(),
                State         = record["State"].As<string?>(),
                Country       = record["Country"].As<string>(),
                Longitude     = lon.HasValue ? (decimal?)Convert.ToDecimal(lon.Value) : null,
                Latitude      = lat.HasValue ? (decimal?)Convert.ToDecimal(lat.Value) : null
            });
        }

        return list;
    }

    // --------------------------------------------------------------------
    // Get trips that a user is buddy on (accepted)
    // --------------------------------------------------------------------
    public async Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId)
    {
        await using var session = _driver.AsyncSession();

        var cypher = @"
            MATCH (u:User { userId: $userId })-[b:BUDDY_ON]->(t:Trip)
            WHERE toLower(b.requestStatus) = 'accepted'
            MATCH (t)-[hs:HAS_STOP { tripDestinationId: b.tripDestinationId }]->(d:Destination)
            RETURN
            t.tripId             AS TripId,
            hs.tripDestinationId AS TripDestinationId,
            d.name               AS DestinationName,
            coalesce(t.description, '') AS TripDescription,
            hs.startDate         AS StartDate,
            hs.endDate           AS EndDate,
            coalesce(t.isArchived, false) AS IsArchived
            ORDER BY hs.startDate
            ";

        var cursor = await session.RunAsync(cypher, new { userId });
        var records = await cursor.ToListAsync();

        var result = new List<BuddyTripSummary>();

        foreach (var record in records)
        {
            result.Add(new BuddyTripSummary
            {
                TripId            = record["TripId"].As<int>(),
                TripDestinationId = record["TripDestinationId"].As<int>(),
                DestinationName   = record["DestinationName"].As<string>(),
                TripDescription   = record["TripDescription"].As<string>(),
                StartDate         = ParseIsoDate(record["StartDate"]),
                EndDate           = ParseIsoDate(record["EndDate"]),
                IsArchived        = record["IsArchived"].As<bool>()
            });
        }

        return result;
    }

    // --------------------------------------------------------------------
    // Trip destination info (with buddies even if buddyId is null in Neo4j)
    // --------------------------------------------------------------------
    public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
    {
        await using var session = _driver.AsyncSession();

        // 1) Header / main info
        var headerCypher = @"
            MATCH (t:Trip)-[hs:HAS_STOP { tripDestinationId: $tripDestinationId }]->(d:Destination)
            OPTIONAL MATCH (owner:User)-[:OWNS]->(t)
            RETURN
            hs.tripDestinationId                 AS TripDestinationId,
            hs.startDate                         AS DestinationStartDate,
            hs.endDate                           AS DestinationEndDate,
            hs.description                       AS DestinationDescription,
            coalesce(hs.isArchived, false)       AS DestinationIsArchived,
            t.tripId                             AS TripId,
            t.maxBuddies                         AS MaxBuddies,
            d.destinationId                      AS DestinationId,
            d.name                               AS DestinationName,
            d.state                              AS DestinationState,
            d.country                            AS DestinationCountry,
            d.longitude                          AS Longitude,
            d.latitude                           AS Latitude,
            owner.userId                         AS OwnerUserId,
            owner.name                           AS OwnerName
            ";

        var headerCursor = await session.RunAsync(headerCypher, new { tripDestinationId });
        var headerRecords = await headerCursor.ToListAsync();
        var headerRecord = headerRecords.SingleOrDefault();

        if (headerRecord == null)
            return null;

        double? lon = headerRecord["Longitude"].As<double?>();
        double? lat = headerRecord["Latitude"].As<double?>();

        var info = new TripDestinationInfo
        {
            TripDestinationId      = headerRecord["TripDestinationId"].As<int>(),
            DestinationStartDate   = ParseIsoDate(headerRecord["DestinationStartDate"]),
            DestinationEndDate     = ParseIsoDate(headerRecord["DestinationEndDate"]),
            DestinationDescription = headerRecord["DestinationDescription"].As<string?>(),
            DestinationIsArchived  = headerRecord["DestinationIsArchived"].As<bool?>(),

            TripId     = headerRecord["TripId"].As<int>(),
            MaxBuddies = headerRecord["MaxBuddies"].As<int?>(),

            DestinationId      = headerRecord["DestinationId"].As<int>(),
            DestinationName    = headerRecord["DestinationName"].As<string>(),
            DestinationState   = headerRecord["DestinationState"].As<string?>(),
            DestinationCountry = headerRecord["DestinationCountry"].As<string>(),
            Longitude          = lon.HasValue ? (decimal?)Convert.ToDecimal(lon.Value) : null,
            Latitude           = lat.HasValue ? (decimal?)Convert.ToDecimal(lat.Value) : null,

            OwnerUserId        = headerRecord["OwnerUserId"].As<int?>() ?? 0,
            OwnerName          = headerRecord["OwnerName"].As<string?>() ?? string.Empty,

            GroupConversationId = null,
            AcceptedBuddies     = new List<BuddyInfo>(),
            PendingRequests     = new List<BuddyRequestInfo>()
        };

        // 2) Accepted buddies – allow null buddyId, fallback to 0
        var acceptedCypher = @"
            MATCH (u:User)-[b:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t:Trip)
            WHERE toLower(b.requestStatus) = 'accepted'
            AND coalesce(b.isActive, true)
            RETURN
            b.buddyId                      AS BuddyId,
            coalesce(b.personCount, 1)     AS PersonCount,
            b.note                         AS BuddyNote,
            u.userId                       AS BuddyUserId,
            u.name                         AS BuddyName
            ";

        var acceptedCursor = await session.RunAsync(acceptedCypher, new { tripDestinationId });
        var acceptedRecords = await acceptedCursor.ToListAsync();

        foreach (var r in acceptedRecords)
        {
            int? buddyIdNullable = r["BuddyId"].As<int?>();
            int buddyId = buddyIdNullable ?? 0; // fallback for migrated data

            info.AcceptedBuddies.Add(new BuddyInfo
            {
                BuddyId     = buddyId,
                PersonCount = r["PersonCount"].As<int>(),
                BuddyNote   = r["BuddyNote"].As<string?>(),
                BuddyUserId = r["BuddyUserId"].As<int>(),
                BuddyName   = r["BuddyName"].As<string>()
            });
        }

        // 3) Pending requests – same trick for BuddyId
        var pendingCypher = @"
            MATCH (u:User)-[b:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t:Trip)
            WHERE toLower(b.requestStatus) = 'pending'
            RETURN
            b.buddyId                      AS BuddyId,
            coalesce(b.personCount, 1)     AS PersonCount,
            b.note                         AS BuddyNote,
            u.userId                       AS RequesterUserId,
            u.name                         AS RequesterName
            ";

        var pendingCursor = await session.RunAsync(pendingCypher, new { tripDestinationId });
        var pendingRecords = await pendingCursor.ToListAsync();

        foreach (var r in pendingRecords)
        {
            int? buddyIdNullable = r["BuddyId"].As<int?>();
            int buddyId = buddyIdNullable ?? 0;

            info.PendingRequests.Add(new BuddyRequestInfo
            {
                BuddyId         = buddyId,
                PersonCount     = r["PersonCount"].As<int>(),
                BuddyNote       = r["BuddyNote"].As<string?>(),
                RequesterUserId = r["RequesterUserId"].As<int>(),
                RequesterName   = r["RequesterName"].As<string>()
            });
        }

        return info;
    }

    // --------------------------------------------------------------------
    // Full trip overview (header + destinations with counts)
    // --------------------------------------------------------------------
    public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
    {
        await using var session = _driver.AsyncSession();

        // Header: use min/max of HAS_STOP dates like the SQL view
        var headerCypher = @"
            MATCH (owner:User)-[:OWNS]->(t:Trip { tripId: $tripId })
            MATCH (t)-[hs:HAS_STOP]->(:Destination)
            WITH owner, t,
                min(hs.startDate) AS TripStart,
                max(hs.endDate)   AS TripEnd
            RETURN
            t.tripId                         AS TripId,
            coalesce(t.tripName, '')         AS TripName,
            TripStart                        AS TripStartDate,
            TripEnd                          AS TripEndDate,
            coalesce(t.maxBuddies, 0)        AS MaxBuddies,
            coalesce(t.description, '')      AS TripDescription,
            owner.userId                     AS OwnerUserId,
            owner.name                       AS OwnerName
            LIMIT 1
            ";

        var headerCursor = await session.RunAsync(headerCypher, new { tripId });
        var headerRecords = await headerCursor.ToListAsync();
        var header = headerRecords.SingleOrDefault();

        if (header == null)
            return null;

        var overview = new TripOverview
        {
            TripId          = header["TripId"].As<int>(),
            TripName        = header["TripName"].As<string>(),
            TripStartDate   = ParseIsoDate(header["TripStartDate"]),
            TripEndDate     = ParseIsoDate(header["TripEndDate"]),
            MaxBuddies      = header["MaxBuddies"].As<int>(),
            TripDescription = header["TripDescription"].As<string>(),
            OwnerUserId     = header["OwnerUserId"].As<int>(),
            OwnerName       = header["OwnerName"].As<string>(),
            Destinations    = new List<SimplifiedTripDestination>()
        };

        // Destinations + accepted persons per destination
        var destCypher = @"
            MATCH (t:Trip { tripId: $tripId })-[hs:HAS_STOP]->(d:Destination)
            OPTIONAL MATCH (u:User)-[b:BUDDY_ON { tripDestinationId: hs.tripDestinationId }]->(t)
            WHERE toLower(b.requestStatus) = 'accepted' AND coalesce(b.isActive, true)
            WITH t, hs, d,
                coalesce(sum(coalesce(b.personCount, 1)), 0) AS acceptedPersons
            RETURN
            t.tripId             AS TripId,
            hs.tripDestinationId AS TripDestinationId,
            hs.startDate         AS DestinationStartDate,
            hs.endDate           AS DestinationEndDate,
            d.name               AS DestinationName,
            d.state              AS DestinationState,
            d.country            AS DestinationCountry,
            coalesce(t.maxBuddies, 0) AS MaxBuddies,
            acceptedPersons      AS AcceptedPersons
            ORDER BY hs.startDate
            ";

        var destCursor = await session.RunAsync(destCypher, new { tripId });
        var destRecords = await destCursor.ToListAsync();

        var dests = new List<SimplifiedTripDestination>();

        foreach (var r in destRecords)
        {
            dests.Add(new SimplifiedTripDestination
            {
                TripDestinationId    = r["TripDestinationId"].As<int>(),
                TripId               = r["TripId"].As<int>(),
                DestinationStartDate = ParseIsoDate(r["DestinationStartDate"]),
                DestinationEndDate   = ParseIsoDate(r["DestinationEndDate"]),
                DestinationName      = r["DestinationName"].As<string>(),
                DestinationState     = r["DestinationState"].As<string?>(),
                DestinationCountry   = r["DestinationCountry"].As<string>(),
                MaxBuddies           = r["MaxBuddies"].As<int>(),
                AcceptedBuddiesCount = r["AcceptedPersons"].As<int>()
            });
        }

        overview.Destinations = dests;
        return overview;
    }

    // --------------------------------------------------------------------
    // All trip overviews owned by a user
    // --------------------------------------------------------------------
    public async Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId)
    {
        await using var session = _driver.AsyncSession();

        // First get all trip headers for trips owned by user
        var headerCypher = @"
            MATCH (owner:User { userId: $userId })-[:OWNS]->(t:Trip)
            MATCH (t)-[hs:HAS_STOP]->(:Destination)
            WITH owner, t,
                min(hs.startDate) AS TripStart,
                max(hs.endDate)   AS TripEnd
            RETURN
            t.tripId                         AS TripId,
            coalesce(t.tripName, '')         AS TripName,
            TripStart                        AS TripStartDate,
            TripEnd                          AS TripEndDate,
            coalesce(t.maxBuddies, 0)        AS MaxBuddies,
            coalesce(t.description, '')      AS TripDescription,
            owner.userId                     AS OwnerUserId,
            owner.name                       AS OwnerName
            ORDER BY TripStartDate
            ";

        var headerCursor = await session.RunAsync(headerCypher, new { userId });
        var headerRecords = await headerCursor.ToListAsync();

        if (!headerRecords.Any())
            return new List<TripOverview>();

        var tripIds = headerRecords
            .Select(r => r["TripId"].As<int>())
            .Distinct()
            .ToList();

        // Now get all destinations for those trips in one go
        var destCypher = @"
            MATCH (t:Trip)-[hs:HAS_STOP]->(d:Destination)
            WHERE t.tripId IN $tripIds
            OPTIONAL MATCH (u:User)-[b:BUDDY_ON { tripDestinationId: hs.tripDestinationId }]->(t)
            WHERE toLower(b.requestStatus) = 'accepted' AND coalesce(b.isActive, true)
            WITH t, hs, d,
                coalesce(sum(coalesce(b.personCount, 1)), 0) AS acceptedPersons
            RETURN
            t.tripId             AS TripId,
            hs.tripDestinationId AS TripDestinationId,
            hs.startDate         AS DestinationStartDate,
            hs.endDate           AS DestinationEndDate,
            d.name               AS DestinationName,
            d.state              AS DestinationState,
            d.country            AS DestinationCountry,
            coalesce(t.maxBuddies, 0) AS MaxBuddies,
            acceptedPersons      AS AcceptedPersons
            ORDER BY t.tripId, hs.startDate
            ";

        var destCursor = await session.RunAsync(destCypher, new { tripIds });
        var destRecords = await destCursor.ToListAsync();

        // Group destinations by TripId
        var destLookup = destRecords
            .GroupBy(r => r["TripId"].As<int>())
            .ToDictionary(
                g => g.Key,
                g => g.Select(r => new SimplifiedTripDestination
                {
                    TripDestinationId    = r["TripDestinationId"].As<int>(),
                    TripId               = r["TripId"].As<int>(),
                    DestinationStartDate = ParseIsoDate(r["DestinationStartDate"]),
                    DestinationEndDate   = ParseIsoDate(r["DestinationEndDate"]),
                    DestinationName      = r["DestinationName"].As<string>(),
                    DestinationState     = r["DestinationState"].As<string?>(),
                    DestinationCountry   = r["DestinationCountry"].As<string>(),
                    MaxBuddies           = r["MaxBuddies"].As<int>(),
                    AcceptedBuddiesCount = r["AcceptedPersons"].As<int>()
                }).ToList()
            );

        var overviews = new List<TripOverview>();

        foreach (var h in headerRecords)
        {
            var id = h["TripId"].As<int>();

            overviews.Add(new TripOverview
            {
                TripId          = id,
                TripName        = h["TripName"].As<string>(),
                TripStartDate   = ParseIsoDate(h["TripStartDate"]),
                TripEndDate     = ParseIsoDate(h["TripEndDate"]),
                MaxBuddies      = h["MaxBuddies"].As<int>(),
                TripDescription = h["TripDescription"].As<string>(),
                OwnerUserId     = h["OwnerUserId"].As<int>(),
                OwnerName       = h["OwnerName"].As<string>(),
                Destinations    = destLookup.TryGetValue(id, out var dests)
                    ? dests
                    : new List<SimplifiedTripDestination>()
            });
        }

        return overviews;
    }

    // --------------------------------------------------------------------
    // Get owner of a specific trip destination
    // --------------------------------------------------------------------
    public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
    {
        await using var session = _driver.AsyncSession();

        var cypher = @"
            MATCH (owner:User)-[:OWNS]->(t:Trip)-[hs:HAS_STOP { tripDestinationId: $tripDestinationId }]->(:Destination)
            RETURN owner.userId AS OwnerId
            LIMIT 1
            ";

        var cursor = await session.RunAsync(cypher, new { tripDestinationId });
        var records = await cursor.ToListAsync();
        var record = records.SingleOrDefault();

        if (record == null)
            return null;

        var ownerValue = record["OwnerId"];
        if (ownerValue == null)
            return null;

        return record["OwnerId"].As<int>();
    }

    // --------------------------------------------------------------------
    // Create a new trip with destinations (not implemented)
    // --------------------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(
    CreateTripWithDestinationsDto dto)
    {
        await using var session = _driver.AsyncSession();

        try
        {
            // 1) Calculate next IDs (trip, tripDestination, destination)
            var tripIdCursor = await session.RunAsync(@"
                MATCH (t:Trip)
                RETURN coalesce(max(t.tripId), 0) AS MaxId
            ");
            var tripIdRecord  = await tripIdCursor.SingleAsync();
            var nextTripId    = tripIdRecord["MaxId"].As<int>() + 1;

            var tripDestIdCursor = await session.RunAsync(@"
                MATCH ()-[hs:HAS_STOP]->()
                RETURN coalesce(max(hs.tripDestinationId), 0) AS MaxId
            ");
            var tripDestIdRecord   = await tripDestIdCursor.SingleAsync();
            var nextTripDestId     = tripDestIdRecord["MaxId"].As<int>() + 1;

            var destIdCursor = await session.RunAsync(@"
                MATCH (d:Destination)
                RETURN coalesce(max(d.destinationId), 0) AS MaxId
            ");
            var destIdRecord      = await destIdCursor.SingleAsync();
            var nextDestinationId = destIdRecord["MaxId"].As<int>() + 1;

            // Guard: must have at least one destination
            if (dto.TripDestinations == null || !dto.TripDestinations.Any())
            {
                return (false, "At least one trip destination is required.");
            }

            var t = dto.CreateTrip;

            // 2) Wrap all writes in a transaction
            var tx = await session.BeginTransactionAsync();
            try
            {
                // 2a) Create Trip node and OWNS relationship
                await tx.RunAsync(@"
                    MATCH (u:User { userId: $ownerId })
                    CREATE (t:Trip {
                        tripId:     $tripId,
                        tripName:   $tripName,
                        maxBuddies: $maxBuddies,
                        startDate:  date($startDate),
                        endDate:    date($endDate),
                        description:$description,
                        isArchived: false
                    })
                    MERGE (u)-[:OWNS]->(t)
                    ",
                    new
                    {
                        ownerId    = t.OwnerId,
                        tripId     = nextTripId,
                        tripName   = t.TripName,
                        maxBuddies = t.MaxBuddies,
                        startDate  = t.StartDate.ToString("yyyy-MM-dd"),
                        endDate    = t.EndDate.ToString("yyyy-MM-dd"),
                        description= t.Description ?? string.Empty
                    });

                // 2b) Create/merge destinations + HAS_STOP relationships
                int sequence = 1;
                int currentTripDestId = nextTripDestId;
                int currentDestId     = nextDestinationId;

                foreach (var d in dto.TripDestinations)
                {
                    // --- ensure we have a Destination node ---
                    int destinationId;
                    if (d.DestinationId.HasValue && d.DestinationId.Value > 0)
                    {
                        destinationId = d.DestinationId.Value;

                        // MERGE by destinationId, set properties on create
                        await tx.RunAsync(@"
                            MERGE (dest:Destination { destinationId: $destinationId })
                            ON CREATE SET
                                dest.name      = $name,
                                dest.state     = $state,
                                dest.country   = $country,
                                dest.longitude = $longitude,
                                dest.latitude  = $latitude
                            ",
                            new
                            {
                                destinationId,
                                name      = d.Name ?? "Unnamed",
                                state     = d.State,
                                country   = d.Country ?? string.Empty,
                                longitude = d.Longitude,
                                latitude  = d.Latitude
                            });
                    }
                    else
                    {
                        destinationId = currentDestId++;

                        await tx.RunAsync(@"
                            CREATE (dest:Destination {
                                destinationId: $destinationId,
                                name:          $name,
                                state:         $state,
                                country:       $country,
                                longitude:     $longitude,
                                latitude:      $latitude
                            })
                            ",
                            new
                            {
                                destinationId,
                                name      = d.Name ?? "Unnamed",
                                state     = d.State,
                                country   = d.Country ?? string.Empty,
                                longitude = d.Longitude,
                                latitude  = d.Latitude
                            });
                    }

                    // --- create HAS_STOP relationship ---
                    var seqNumber = d.SequenceNumber == 0 ? sequence : d.SequenceNumber;

                    await tx.RunAsync(@"
                        MATCH (t:Trip { tripId: $tripId })
                        MATCH (dest:Destination { destinationId: $destinationId })
                        CREATE (t)-[:HAS_STOP {
                            tripDestinationId: $tripDestinationId,
                            startDate:         date($startDate),
                            endDate:           date($endDate),
                            sequenceNumber:    $sequenceNumber,
                            description:       $description
                        }]->(dest)
                        ",
                        new
                        {
                            tripId            = nextTripId,
                            destinationId,
                            tripDestinationId = currentTripDestId++,
                            startDate         = d.DestinationStartDate.ToString("yyyy-MM-dd"),
                            endDate           = d.DestinationEndDate.ToString("yyyy-MM-dd"),
                            sequenceNumber    = seqNumber,
                            description       = d.Description ?? string.Empty
                        });

                    sequence++;
                }

                await tx.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                return (false, "Error creating trip with destinations in Neo4j: " + ex.Message);
            }
        }
        catch (Exception exOuter)
        {
            return (false, "Error (outer) creating trip with destinations in Neo4j: " + exOuter.Message);
        }
    }

    // --------------------------------------------------------------------
    // User leaves a trip destination (deactivate buddy)
    // --------------------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
        int userId,
        int tripDestinationId,
        int changedBy,
        string departureReason)
    {
        await using var session = _driver.AsyncSession();

        var cypher = @"
            MATCH (u:User { userId: $userId })-[b:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t:Trip)
            WHERE toLower(b.requestStatus) = 'accepted'
            SET b.isActive = false,
                b.departureReason = $departureReason
            RETURN count(b) AS UpdatedCount
            ";

        var cursor = await session.RunAsync(cypher, new
        {
            userId,
            tripDestinationId,
            departureReason
        });

        var record = await cursor.SingleAsync();
        var updatedCount = record["UpdatedCount"].As<long>();

        if (updatedCount == 0)
        {
            return (false, "Buddy not found for this trip destination.");
        }

        return (true, null);
    }

    // --------------------------------------------------------------------
    // Pending buddy requests for a user (as owner)
    // --------------------------------------------------------------------
    public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
{
    await using var session = _driver.AsyncSession();

    var cypher = @"
MATCH (owner:User { userId: $userId })-[:OWNS]->(t:Trip)-[hs:HAS_STOP]->(d:Destination)
MATCH (requester:User)-[b:BUDDY_ON { tripDestinationId: hs.tripDestinationId }]->(t)
WHERE toLower(trim(b.requestStatus)) = 'pending'
RETURN
  hs.tripDestinationId AS TripDestinationId,
  d.name               AS DestinationName,
  hs.startDate         AS DestinationStartDate,
  hs.endDate           AS DestinationEndDate,
  t.tripId             AS TripId,
  b.buddyId            AS BuddyId,
  requester.userId     AS RequesterUserId,
  requester.name       AS RequesterName,
  b.note               AS BuddyNote,
  coalesce(b.personCount, 1) AS PersonCount
ORDER BY hs.startDate
";

    var cursor = await session.RunAsync(cypher, new { userId });
    var records = await cursor.ToListAsync();

    var list = new List<PendingBuddyRequest>();

    foreach (var r in records)
    {
        int? buddyIdNullable = r["BuddyId"].As<int?>();
        int buddyId = buddyIdNullable ?? 0; // fallback for migrated data with no buddyId

        list.Add(new PendingBuddyRequest
        {
            TripDestinationId    = r["TripDestinationId"].As<int>(),
            DestinationName      = r["DestinationName"].As<string>(),
            DestinationStartDate = ParseIsoDate(r["DestinationStartDate"]),
            DestinationEndDate   = ParseIsoDate(r["DestinationEndDate"]),
            TripId               = r["TripId"].As<int>(),
            BuddyId              = buddyId,
            RequesterUserId      = r["RequesterUserId"].As<int>(),
            RequesterName        = r["RequesterName"].As<string>(),
            BuddyNote            = r["BuddyNote"].As<string?>(),
            PersonCount          = r["PersonCount"].As<int>()
        });
    }

    return list;
}

    // --------------------------------------------------------------------
    // Insert a buddy request
    // --------------------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        await using var session = _driver.AsyncSession();

        // Check capacity before creating request
        var checkCapacityQuery = @"
            MATCH (t:Trip)-[hs:HAS_STOP { tripDestinationId: $tripDestinationId }]->(:Destination)
            OPTIONAL MATCH (accepted:User)-[ab:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t)
            WHERE toLower(ab.requestStatus) = 'accepted' AND coalesce(ab.isActive, true)
            WITH t, coalesce(sum(coalesce(ab.personCount, 1)), 0) AS acceptedPersons
            RETURN 
                t.maxBuddies AS MaxBuddies,
                acceptedPersons AS AcceptedPersons
            ";

        var capacityCursor = await session.RunAsync(checkCapacityQuery, new { tripDestinationId = buddyDto.TripDestinationId });
        var capacityRecords = await capacityCursor.ToListAsync();
        var capacityRecord = capacityRecords.SingleOrDefault();

        if (capacityRecord == null)
        {
            return (false, "Trip destination not found");
        }

        var maxBuddies = capacityRecord["MaxBuddies"].As<int?>();
        var acceptedPersons = capacityRecord["AcceptedPersons"].As<int>();

        if (maxBuddies.HasValue)
        {
            var remaining = maxBuddies.Value - acceptedPersons;
            var requestedCount = buddyDto.PersonCount;

            if (remaining < requestedCount)
            {
                return (false, "there is not enough buddy capacity for the person_count");
            }
        }

        var newBuddyId = await GetNextBuddyIdAsync();

        var cypher = @"
MATCH (u:User { userId: $userId })
MATCH (t:Trip)-[hs:HAS_STOP { tripDestinationId: $tripDestinationId }]->(:Destination)
MERGE (u)-[b:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t)
ON CREATE SET
    b.buddyId       = $buddyId,
    b.personCount   = $personCount,
    b.note          = $note,
    b.requestStatus = 'pending',
    b.isActive      = false
ON MATCH SET
    b.personCount   = $personCount,
    b.note          = $note,
    b.requestStatus = 'pending',
    b.isActive      = false
RETURN b.buddyId AS BuddyId
";

        var createCursor = await session.RunAsync(cypher, new
        {
            tripDestinationId = buddyDto.TripDestinationId,
            userId            = buddyDto.UserId,
            buddyId           = newBuddyId,
            personCount       = buddyDto.PersonCount,
            note              = buddyDto.Note
        });

        var created = await createCursor.SingleAsync();

        if (created == null)
        {
            return (false, "Failed to create buddy request in Neo4j.");
        }

        return (true, null);
    }

    // --------------------------------------------------------------------
    // Owner accepts/rejects a buddy request
    // --------------------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(
        UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        var newStatus = updateBuddyRequestDto.NewStatus switch
        {
            BuddyRequestUpdateStatus.Accepted => "accepted",
            BuddyRequestUpdateStatus.Rejected => "rejected",
            _ => "pending"
        };

        await using var session = _driver.AsyncSession();

        // If accepting, check capacity first
        if (newStatus == "accepted")
        {
            var checkCapacityQuery = @"
                MATCH (u:User)-[b:BUDDY_ON { buddyId: $buddyId }]->(t:Trip)
                OPTIONAL MATCH (accepted:User)-[ab:BUDDY_ON { tripDestinationId: b.tripDestinationId }]->(t)
                WHERE toLower(ab.requestStatus) = 'accepted' AND coalesce(ab.isActive, true)
                WITH t, b, coalesce(sum(coalesce(ab.personCount, 1)), 0) AS acceptedPersons
                RETURN 
                    t.maxBuddies AS MaxBuddies,
                    acceptedPersons AS AcceptedPersons,
                    b.personCount AS RequestPersonCount
                ";

            var capacityCursor = await session.RunAsync(checkCapacityQuery, new { buddyId = updateBuddyRequestDto.BuddyId });
            var capacityRecords = await capacityCursor.ToListAsync();
            var capacityRecord = capacityRecords.SingleOrDefault();

            if (capacityRecord == null)
            {
                return (false, "Buddy request not found");
            }

            var maxBuddies = capacityRecord["MaxBuddies"].As<int?>();
            var acceptedPersons = capacityRecord["AcceptedPersons"].As<int>();
            var requestPersonCount = capacityRecord["RequestPersonCount"].As<int?>();

            if (maxBuddies.HasValue)
            {
                var remaining = maxBuddies.Value - acceptedPersons;
                var requestedCount = requestPersonCount ?? 1;

                if (remaining < requestedCount)
                {
                    return (false, "Cannot accept request: Trip destination is at maximum capacity");
                }
            }
        }

        var cypher = @"
            MATCH (u:User)-[b:BUDDY_ON { buddyId: $buddyId }]->(t:Trip)
            SET b.requestStatus = $status,
                b.isActive = ($status = 'accepted'),
                b.departureReason = CASE WHEN $status = 'accepted' THEN null ELSE b.departureReason END
            RETURN count(b) AS UpdatedCount
            ";

        var cursor = await session.RunAsync(cypher, new
        {
            buddyId = updateBuddyRequestDto.BuddyId,
            status  = newStatus
        });

        var record = await cursor.SingleAsync();
        var updatedCount = record["UpdatedCount"].As<long>();

        if (updatedCount == 0)
        {
            return (false, "Buddy request not found");
        }

        return (true, null);
    }

    // --------------------------------------------------------------------
    // Audit tables
    // --------------------------------------------------------------------
    public async Task<IEnumerable<TripAudit>> GetTripAuditsAsync()
    {
        await using var session = _driver.AsyncSession();
        const string cypher = @"
            MATCH (t:Trip)-[:HAS_AUDIT]->(a:TripAudit)
            OPTIONAL MATCH (cb:User)-[:CHANGED]->(a)
            RETURN a.auditId as AuditId, t.tripId as TripId, a.action as Action,
                   a.fieldChanged as FieldChanged, a.oldValue as OldValue,
                   a.newValue as NewValue, cb.userId as ChangedBy,
                   a.timestamp as Timestamp
            ORDER BY a.timestamp ASC
        ";
        
        var cursor = await session.RunAsync(cypher);
        var records = await cursor.ToListAsync();
        
        return records.Select(r => new TripAudit
        {
            AuditId = r["AuditId"].As<int?>() ?? 0,
            TripId = r["TripId"].As<int?>() ?? 0,
            Action = r["Action"].As<string?>() ?? string.Empty,
            FieldChanged = r["FieldChanged"].As<string?>(),
            OldValue = r["OldValue"].As<string?>(),
            NewValue = r["NewValue"].As<string?>(),
            ChangedBy = r["ChangedBy"].As<int?>(),
            Timestamp = ParseDateTime(r["Timestamp"])
        }).ToList();
    }

    public async Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync()
    {
        await using var session = _driver.AsyncSession();
        const string cypher = @"
            MATCH (a:BuddyAudit)
            OPTIONAL MATCH (u:User)-[:CHANGED]->(a)
            RETURN a.auditId as AuditId, a.buddyId as BuddyId, a.action as Action,
                   a.reason as Reason, u.userId as ChangedBy,
                   a.timestamp as Timestamp
            ORDER BY a.timestamp ASC
        ";
        
        var cursor = await session.RunAsync(cypher);
        var records = await cursor.ToListAsync();
        
        return records.Select(r => new BuddyAudit
        {
            AuditId = r["AuditId"].As<int?>() ?? 0,
            BuddyId = r["BuddyId"].As<int?>() ?? 0,
            Action = r["Action"].As<string?>() ?? string.Empty,
            Reason = r["Reason"].As<string?>(),
            ChangedBy = r["ChangedBy"].As<int?>(),
            Timestamp = ParseDateTime(r["Timestamp"])
        }).ToList();
    }

    // ------------------------------- ADMIN DELETION METHODS -------------------------------
    public async Task<(bool Success, string? ErrorMessage)> DeleteTripAsync(int tripId, int changedBy)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            return await session.ExecuteWriteAsync(async tx =>
            {
                // Get trip name for audit
                var checkCypher = @"
                    MATCH (t:Trip {tripId: $tripId})
                    RETURN t.tripName as TripName
                ";
                var checkCursor = await tx.RunAsync(checkCypher, new { tripId });
                var checkRecords = await checkCursor.ToListAsync();
                
                if (!checkRecords.Any())
                    return (false, "No trip found with the given trip_id");

                var tripName = checkRecords.First()["TripName"].As<string?>();

                // Get next audit ID
                var auditIdCypher = @"
                    MATCH (a:TripAudit)
                    RETURN coalesce(max(a.auditId), 0) + 1 as NextId
                ";
                var auditIdCursor = await tx.RunAsync(auditIdCypher);
                var auditIdRecord = await auditIdCursor.SingleAsync();
                var nextAuditId = auditIdRecord["NextId"].As<int>();

                // Create audit node
                var auditCypher = @"
                    CREATE (a:TripAudit {
                        auditId: $auditId,
                        tripId: $tripId,
                        action: 'deleted',
                        fieldChanged: 'trip',
                        oldValue: $tripName,
                        newValue: null,
                        timestamp: datetime()
                    })
                    WITH a
                    MATCH (u:User {userId: $changedBy})
                    CREATE (u)-[:CHANGED]->(a)
                ";
                await tx.RunAsync(auditCypher, new { auditId = nextAuditId, tripId, tripName, changedBy });

                // Delete trip and all relationships
                var deleteCypher = @"
                    MATCH (t:Trip {tripId: $tripId})
                    OPTIONAL MATCH (t)-[r]-()
                    DELETE r, t
                ";
                await tx.RunAsync(deleteCypher, new { tripId });

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteTripDestinationAsync(int tripDestinationId, int changedBy)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            return await session.ExecuteWriteAsync(async tx =>
            {
                // Get trip and destination info for audit
                var checkCypher = @"
                    MATCH (t:Trip)-[hs:HAS_STOP {tripDestinationId: $tripDestinationId}]->(d:Destination)
                    RETURN t.tripId as TripId, d.name as DestinationName
                ";
                var checkCursor = await tx.RunAsync(checkCypher, new { tripDestinationId });
                var checkRecords = await checkCursor.ToListAsync();
                
                if (!checkRecords.Any())
                    return (false, "No trip destination found with the given trip_destination_id");

                var tripId = checkRecords.First()["TripId"].As<int>();
                var destinationName = checkRecords.First()["DestinationName"].As<string?>();

                // Get next audit ID
                var auditIdCypher = @"
                    MATCH (a:TripAudit)
                    RETURN coalesce(max(a.auditId), 0) + 1 as NextId
                ";
                var auditIdCursor = await tx.RunAsync(auditIdCypher);
                var auditIdRecord = await auditIdCursor.SingleAsync();
                var nextAuditId = auditIdRecord["NextId"].As<int>();

                // Create audit node
                var auditCypher = @"
                    CREATE (a:TripAudit {
                        auditId: $auditId,
                        tripId: $tripId,
                        action: 'deleted',
                        fieldChanged: 'trip_destination',
                        oldValue: 'Destination: ' + $destinationName,
                        newValue: null,
                        timestamp: datetime()
                    })
                    WITH a
                    MATCH (u:User {userId: $changedBy})
                    CREATE (u)-[:CHANGED]->(a)
                ";
                await tx.RunAsync(auditCypher, new { auditId = nextAuditId, tripId, destinationName, changedBy });

                // Delete the HAS_STOP relationship and any BUDDY_ON relationships
                var deleteCypher = @"
                    MATCH (t:Trip)-[hs:HAS_STOP {tripDestinationId: $tripDestinationId}]->(d:Destination)
                    OPTIONAL MATCH ()-[b:BUDDY_ON {tripDestinationId: $tripDestinationId}]->()
                    DELETE hs, b
                ";
                await tx.RunAsync(deleteCypher, new { tripDestinationId });

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }
    
    public async Task<(bool Success, string? ErrorMessage)> DeleteDestinationAsync(int destinationId, int changedBy)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            return await session.ExecuteWriteAsync(async tx =>
            {
                // Check if destination exists
                var checkCypher = @"
                    MATCH (d:Destination {destinationId: $destinationId})
                    RETURN d.name as DestinationName
                ";
                var checkCursor = await tx.RunAsync(checkCypher, new { destinationId });
                var checkRecords = await checkCursor.ToListAsync();
                
                if (!checkRecords.Any())
                    return (false, "No destination found with the given destination_id");

                // PERMANENTLY delete the destination and all relationships
                var deleteCypher = @"
                    MATCH (d:Destination {destinationId: $destinationId})
                    OPTIONAL MATCH (d)-[r]-()
                    DELETE r, d
                ";
                await tx.RunAsync(deleteCypher, new { destinationId });

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateTripInfoAsync(int tripId, int ownerId, string? tripName, string? description)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            // Only update if at least one field is provided (not null/empty)
            if ((tripName == null || string.IsNullOrEmpty(tripName)) && string.IsNullOrEmpty(description))
                return (true, null);
            
            return await session.ExecuteWriteAsync(async tx =>
            {
                var cypher = @"
                    MATCH (u:User {userId: $ownerId})-[:OWNS]->(t:Trip {tripId: $tripId})
                    WITH t, t.tripName AS oldTripName, t.description AS oldDescription
                    " + ((tripName == null || string.IsNullOrEmpty(tripName)) ? "" : "SET t.tripName = $tripName ") +
                    (string.IsNullOrEmpty(description) ? "" : "SET t.description = $description ") + @"
                    WITH t, oldTripName, oldDescription
                    OPTIONAL MATCH (maxAudit:TripAudit)
                    WITH t, oldTripName, oldDescription, coalesce(max(maxAudit.AuditId), 0) AS maxId
                    " + ((tripName == null || string.IsNullOrEmpty(tripName)) ? "" : @"FOREACH (_ IN CASE WHEN oldTripName <> $tripName THEN [1] ELSE [] END |
                        CREATE (audit:TripAudit {
                            AuditId: maxId + 1,
                            TripId: $tripId,
                            Action: 'updated',
                            FieldChanged: 'trip_name',
                            OldValue: oldTripName,
                            NewValue: $tripName,
                            ChangedBy: $ownerId,
                            Timestamp: datetime()
                        })
                    )
                    ") + (string.IsNullOrEmpty(description) ? "" : @"FOREACH (_ IN CASE WHEN oldDescription <> $description OR (oldDescription IS NULL AND $description IS NOT NULL) OR (oldDescription IS NOT NULL AND $description IS NULL) THEN [1] ELSE [] END |
                        CREATE (audit:TripAudit {
                            AuditId: maxId + 2,
                            TripId: $tripId,
                            Action: 'updated',
                            FieldChanged: 'description',
                            OldValue: oldDescription,
                            NewValue: $description,
                            ChangedBy: $ownerId,
                            Timestamp: datetime()
                        })
                    )
                    ") + @"RETURN count(t) AS updatedCount
                ";

                var cursor = await tx.RunAsync(cypher, new { tripId, ownerId, tripName = (tripName == null || string.IsNullOrEmpty(tripName)) ? null : tripName, description = string.IsNullOrEmpty(description) ? null : description });
                var records = await cursor.ToListAsync();
                
                if (!records.Any() || records[0]["updatedCount"].As<long>() == 0)
                    return (false, "Trip not found or you are not the owner.");

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateTripDestinationDescriptionAsync(int tripDestinationId, int ownerId, string? description)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            // Trip destination description must not be null or empty/whitespace
            if (string.IsNullOrWhiteSpace(description))
                return (false, "Trip destination description cannot be empty or whitespace.");
            
            return await session.ExecuteWriteAsync(async tx =>
            {
                var cypher = @"
                    MATCH (u:User {userId: $ownerId})-[:OWNS]->(t:Trip)-[td:HAS_STOP {tripDestinationId: $tripDestinationId}]->(d:Destination)
                    WITH t, td, td.description AS oldDescription
                    SET td.description = $description
                    WITH t, td, oldDescription
                    OPTIONAL MATCH (maxAudit:TripAudit)
                    WITH t, td, oldDescription, coalesce(max(maxAudit.AuditId), 0) AS maxId
                    FOREACH (_ IN CASE WHEN oldDescription <> $description OR (oldDescription IS NULL AND $description IS NOT NULL) OR (oldDescription IS NOT NULL AND $description IS NULL) THEN [1] ELSE [] END |
                        CREATE (audit:TripAudit {
                            AuditId: maxId + 1,
                            TripId: t.tripId,
                            Action: 'updated',
                            FieldChanged: 'trip_destination_description',
                            OldValue: oldDescription,
                            NewValue: $description,
                            ChangedBy: $ownerId,
                            Timestamp: datetime()
                        })
                    )
                    RETURN count(td) AS updatedCount
                ";

                var cursor = await tx.RunAsync(cypher, new { tripDestinationId, ownerId, description });
                var records = await cursor.ToListAsync();
                
                if (!records.Any() || records[0]["updatedCount"].As<long>() == 0)
                    return (false, "Trip destination not found or you are not the owner.");

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }
    
    private static DateTime ParseDateTime(object value)
    {
        if (value == null)
            return DateTime.MinValue;

        if (value is DateTime dt)
            return dt;

        // Handle Neo4j ZonedDateTime
        if (value is Neo4j.Driver.ZonedDateTime zdt)
            return zdt.ToDateTimeOffset().DateTime;

        // Handle Neo4j LocalDateTime
        if (value is Neo4j.Driver.LocalDateTime ldt)
            return ldt.ToDateTime();

        // Fallback to reflection for other temporal types
        var valueType = value.GetType();
        if (valueType.Name == "LocalDateTime" || valueType.Name == "ZonedDateTime")
        {
            var toDateTimeMethod = valueType.GetMethod("ToDateTime");
            if (toDateTimeMethod != null)
            {
                var result = toDateTimeMethod.Invoke(value, null);
                if (result is DateTime dateTime)
                    return dateTime;
            }
        }

        if (value is string s)
        {
            if (DateTime.TryParse(s, System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
                return parsed;
            return DateTime.MinValue;
        }

        return DateTime.MinValue;
    }
}