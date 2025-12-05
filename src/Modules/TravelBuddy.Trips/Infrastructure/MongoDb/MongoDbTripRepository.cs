using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips
{
    // ---------- Mongo document shapes (match migrator) ----------
    [BsonIgnoreExtraElements]
    internal class UserDocument
    {
        // Map to MongoDB _id (same as in the Users module)
        [BsonId]
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    [BsonIgnoreExtraElements]
    internal class BuddyEmbedded
    {
        public int BuddyId { get; set; }
        public int UserId { get; set; }
        public int? PersonCount { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? DepartureReason { get; set; }
        public string RequestStatus { get; set; } = string.Empty; // "pending", "accepted", "rejected"
    }

    [BsonIgnoreExtraElements]
    internal class TripDestinationEmbedded
    {
        public int TripDestinationId { get; set; }
        public int DestinationId { get; set; }

        // Use DateTime because Mongo stores BSON Date
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int SequenceNumber { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }
        public List<BuddyEmbedded> Buddies { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class TripDocument
    {
        // Map MongoDB _id (int) to TripId
        [BsonId]
        public int TripId { get; set; }
        public string TripName { get; set; } = null!;
        public int? OwnerId { get; set; }
        public int? MaxBuddies { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public string? Description { get; set; }
        public bool? IsArchived { get; set; }

        public List<TripDestinationEmbedded> Destinations { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class DestinationDocument
    {
        [BsonId]
        public int DestinationId { get; set; }

        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class TripAuditDocument
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int AuditId { get; set; }
        public int TripId { get; set; }
        public string Action { get; set; } = null!;
        public string? FieldChanged { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class BuddyAuditDocument
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
        public int AuditId { get; set; }
        public int BuddyId { get; set; }
        public string Action { get; set; } = null!;
        public string? Reason { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    // ---------- Repository implementation ----------

    public class MongoDbTripRepository : ITripRepository
    {
        private readonly IMongoCollection<TripDocument> _tripsCollection;
        private readonly IMongoCollection<DestinationDocument> _destinationsCollection;
        private readonly IMongoCollection<UserDocument> _usersCollection;

        public MongoDbTripRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _tripsCollection = database.GetCollection<TripDocument>("trips");
            _destinationsCollection = database.GetCollection<DestinationDocument>("destinations");
            _usersCollection = database.GetCollection<UserDocument>("users");
        }

        // Helper to load all trips and destinations into memory
        private async Task<(List<TripDocument> Trips, Dictionary<int, DestinationDocument> Destinations)>
            LoadTripsAndDestinationsAsync()
        {
            var tripsTask = _tripsCollection
                .Find(FilterDefinition<TripDocument>.Empty)
                .ToListAsync();

            var destsTask = _destinationsCollection
                .Find(FilterDefinition<DestinationDocument>.Empty)
                .ToListAsync();

            await Task.WhenAll(tripsTask, destsTask);

            var trips = tripsTask.Result;

            // Some destination documents may share the same DestinationId (e.g., 0)
            // so we group by DestinationId and keep only the first for each key.
            var destMap = destsTask.Result
                .GroupBy(d => d.DestinationId)
                .ToDictionary(g => g.Key, g => g.First());

            return (trips, destMap);
        }

        // Helper: load all trips into memory
        private async Task<List<TripDocument>> LoadTripsAsync()
        {
            return await _tripsCollection
                .Find(FilterDefinition<TripDocument>.Empty)
                .ToListAsync();
        }

        // Helper: generate next BuddyId (global)
        private async Task<int> GetNextBuddyIdAsync()
        {
            var trips = await LoadTripsAsync();
            var max = trips
                .SelectMany(t => t.Destinations)
                .SelectMany(td => td.Buddies)
                .DefaultIfEmpty()
                .Max(b => b?.BuddyId ?? 0);

            return max + 1;
        }

        // ---------------------------------------------------------
        // Search available trips / destinations
        // ---------------------------------------------------------
        public async Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var (trips, destMap) = await LoadTripsAndDestinationsAsync();

            var results = new List<TripDestinationSearchResult>();

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    var destStart = DateOnly.FromDateTime(td.StartDate);
                    var destEnd = DateOnly.FromDateTime(td.EndDate);

                    // --- date overlap filter ---
                    if (reqStart.HasValue && destEnd < reqStart.Value)
                        continue;
                    if (reqEnd.HasValue && destStart > reqEnd.Value)
                        continue;

                    // --- country/state filter (case-insensitive) ---
                    if (!string.IsNullOrWhiteSpace(country) &&
                        !string.Equals(destDoc.Country, country, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!string.IsNullOrWhiteSpace(state) &&
                        !string.Equals(destDoc.State ?? string.Empty, state, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // --- name / q filter ---
                    var nameLower = name?.ToLowerInvariant();
                    var qLower = q?.ToLowerInvariant();

                    if (!string.IsNullOrWhiteSpace(nameLower))
                    {
                        if (!destDoc.Name.ToLowerInvariant().Contains(nameLower))
                            continue;
                    }

                    if (!string.IsNullOrWhiteSpace(qLower))
                    {
                        var combined = $"{destDoc.Name} {destDoc.Country} {destDoc.State} {trip.Description}".ToLowerInvariant();
                        if (!combined.Contains(qLower))
                            continue;
                    }

                    // --- capacity calculation ---
                    var maxBuddies = trip.MaxBuddies ?? 0;
                    var acceptedPersons = td.Buddies
                        .Where(b => string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                        .Sum(b => b.PersonCount ?? 1);

                    var remaining = maxBuddies > 0
                        ? Math.Max(0, maxBuddies - acceptedPersons)
                        : 0;

                    if (partySize.HasValue && remaining < partySize.Value)
                        continue;

                    results.Add(new TripDestinationSearchResult
                    {
                        TripDestinationId = td.TripDestinationId,
                        TripId = trip.TripId,
                        DestinationId = td.DestinationId,
                        DestinationName = destDoc.Name,
                        Country = destDoc.Country,
                        State = destDoc.State ?? string.Empty,
                        DestinationStart = destStart,
                        DestinationEnd = destEnd,
                        MaxBuddies = maxBuddies,
                        AcceptedPersons = acceptedPersons,
                        RemainingCapacity = remaining
                    });
                }
            }

            return results;
        }

        public async Task<IEnumerable<Destination>> GetDestinationsAsync()
        {
            var docs = await _destinationsCollection
                .Find(FilterDefinition<DestinationDocument>.Empty)
                .ToListAsync();

            // If there happen to be duplicate DestinationIds in Mongo, keep the first
            var distinctDocs = docs
                .GroupBy(d => d.DestinationId)
                .Select(g => g.First())
                .ToList();

            var destinations = distinctDocs.Select(d => new Destination
            {
                DestinationId = d.DestinationId,
                Name          = d.Name,
                State         = d.State,
                Country       = d.Country,
                Longitude     = d.Longitude,
                Latitude      = d.Latitude
            });

            return destinations;
        }


        // ---------------------------------------------------------
        // Get trips that a user owns or is buddy on
        // ---------------------------------------------------------
        public async Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId)
        {
            var (trips, destMap) = await LoadTripsAndDestinationsAsync();
            var summaries = new List<BuddyTripSummary>();

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    if (!td.Buddies.Any(b =>
                            b.UserId == userId &&
                            string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase)
                        ))
                        continue;

                    summaries.Add(new BuddyTripSummary
                    {
                        TripId = trip.TripId,
                        TripDestinationId = td.TripDestinationId,
                        DestinationName = destDoc.Name,
                        TripDescription = trip.Description ?? string.Empty
                    });
                }
            }

            return summaries;
        }

        // ---------------------------------------------------------
        // Get trip destination info by id
        // ---------------------------------------------------------
        public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
        {
            // Find the trip that contains this trip destination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == tripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return null;

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == tripDestinationId);
            if (td == null)
                return null;

            // Load destination info
            var destDoc = await _destinationsCollection
                .Find(d => d.DestinationId == td.DestinationId)
                .FirstOrDefaultAsync();

            if (destDoc == null)
                return null;

            // Load all users that are buddies on this destination (accepted + pending)
            var buddyUserIds = td.Buddies
                .Select(b => b.UserId)
                .Distinct()
                .ToList();

            var buddyUserDocs = await _usersCollection
                .Find(u => buddyUserIds.Contains(u.UserId))
                .ToListAsync();

            var userMap = buddyUserDocs.ToDictionary(u => u.UserId, u => u);

            // Load owner (if any)
            UserDocument? ownerDoc = null;
            if (trip.OwnerId.HasValue)
            {
                ownerDoc = await _usersCollection
                    .Find(u => u.UserId == trip.OwnerId.Value)
                    .FirstOrDefaultAsync();
            }

            var info = new TripDestinationInfo
            {
                TripDestinationId    = td.TripDestinationId,
                DestinationStartDate = DateOnly.FromDateTime(td.StartDate),
                DestinationEndDate   = DateOnly.FromDateTime(td.EndDate),
                DestinationDescription = td.Description,
                DestinationIsArchived  = td.IsArchived,

                TripId      = trip.TripId,
                MaxBuddies  = trip.MaxBuddies,

                DestinationId      = destDoc.DestinationId,
                DestinationName    = destDoc.Name,
                DestinationState   = destDoc.State,
                DestinationCountry = destDoc.Country,
                Longitude          = destDoc.Longitude,
                Latitude           = destDoc.Latitude,

                OwnerUserId = trip.OwnerId ?? 0,
                OwnerName   = ownerDoc?.Name ?? string.Empty,

                // There is no group conversation in Mongo model right now
                GroupConversationId = null
            };

            foreach (var b in td.Buddies)
            {
                userMap.TryGetValue(b.UserId, out var uDoc);

                if (string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                {
                    info.AcceptedBuddies.Add(new BuddyInfo
                    {
                        BuddyId      = b.BuddyId,
                        PersonCount  = b.PersonCount ?? 1,
                        BuddyNote    = b.Note,
                        BuddyUserId  = b.UserId,
                        BuddyName    = uDoc?.Name ?? string.Empty
                    });
                }
                else if (string.Equals(b.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase))
                {
                    info.PendingRequests.Add(new BuddyRequestInfo
                    {
                        BuddyId         = b.BuddyId,
                        PersonCount     = b.PersonCount ?? 1,
                        BuddyNote       = b.Note,
                        RequesterUserId = b.UserId,
                        RequesterName   = uDoc?.Name ?? string.Empty
                    });
                }
                // We ignore "rejected" here – same as the relational logic normally does
            }

            return info;
        }



        // ---------------------------------------------------------
        // Get full trip overview
        // ---------------------------------------------------------
        public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
        {
            var trip = await _tripsCollection
                .Find(t => t.TripId == tripId)
                .FirstOrDefaultAsync();

            if (trip == null)
                return null;

            // Load owner
            UserDocument? ownerDoc = null;
            if (trip.OwnerId.HasValue)
            {
                ownerDoc = await _usersCollection
                    .Find(u => u.UserId == trip.OwnerId.Value)
                    .FirstOrDefaultAsync();
            }

            // Load all destinations used in this trip
            var destIds = trip.Destinations
                .Select(td => td.DestinationId)
                .Distinct()
                .ToList();

            var destDocs = await _destinationsCollection
                .Find(d => destIds.Contains(d.DestinationId))
                .ToListAsync();

            var destMap = destDocs.ToDictionary(d => d.DestinationId, d => d);

            var overview = new TripOverview
            {
                TripId          = trip.TripId,
                TripName        = trip.TripName,
                TripStartDate   = trip.StartDate,
                TripEndDate     = trip.EndDate,
                MaxBuddies      = trip.MaxBuddies ?? 0,
                TripDescription = trip.Description ?? string.Empty,
                OwnerUserId     = trip.OwnerId ?? 0,
                OwnerName       = ownerDoc?.Name ?? string.Empty,
                Destinations    = new List<SimplifiedTripDestination>()
            };

            foreach (var td in trip.Destinations.OrderBy(td => td.SequenceNumber))
            {
                if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                    continue;

                var acceptedCount = td.Buddies
                    .Where(b => string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                    .Sum(b => b.PersonCount ?? 1);

                overview.Destinations.Add(new SimplifiedTripDestination
                {
                    TripDestinationId    = td.TripDestinationId,
                    TripId               = trip.TripId,
                    DestinationStartDate = DateOnly.FromDateTime(td.StartDate),
                    DestinationEndDate   = DateOnly.FromDateTime(td.EndDate),
                    DestinationName      = destDoc.Name,
                    DestinationState     = destDoc.State,
                    DestinationCountry   = destDoc.Country,
                    MaxBuddies           = trip.MaxBuddies ?? 0,
                    AcceptedBuddiesCount = acceptedCount
                });
            }

            return overview;
        }



        // ---------------------------------------------------------
        // Get all trip overviews owned by a user
        // ---------------------------------------------------------
        public async Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId)
        {
            // All trips owned by this user
            var trips = await _tripsCollection
                .Find(t => t.OwnerId == userId)
                .ToListAsync();

            if (!trips.Any())
                return new List<TripOverview>();

            // Load owner once
            var ownerDoc = await _usersCollection
                .Find(u => u.UserId == userId)
                .FirstOrDefaultAsync();

            // Preload all destinations used in these trips
            var destIds = trips
                .SelectMany(t => t.Destinations)
                .Select(td => td.DestinationId)
                .Distinct()
                .ToList();

            var destDocs = await _destinationsCollection
                .Find(d => destIds.Contains(d.DestinationId))
                .ToListAsync();

            var destMap = destDocs.ToDictionary(d => d.DestinationId, d => d);

            var result = new List<TripOverview>();

            foreach (var trip in trips)
            {
                var overview = new TripOverview
                {
                    TripId          = trip.TripId,
                    TripName        = trip.TripName, // or trip.TripName if present
                    TripStartDate   = trip.StartDate,
                    TripEndDate     = trip.EndDate,
                    MaxBuddies      = trip.MaxBuddies ?? 0,
                    TripDescription = trip.Description ?? string.Empty,
                    OwnerUserId     = userId,
                    OwnerName       = ownerDoc?.Name ?? string.Empty,
                    Destinations    = new List<SimplifiedTripDestination>()
                };

                foreach (var td in trip.Destinations.OrderBy(td => td.SequenceNumber))
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    var acceptedCount = td.Buddies
                        .Where(b => string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                        .Sum(b => b.PersonCount ?? 1);

                    overview.Destinations.Add(new SimplifiedTripDestination
                    {
                        TripDestinationId    = td.TripDestinationId,
                        TripId               = trip.TripId,
                        DestinationStartDate = DateOnly.FromDateTime(td.StartDate),
                        DestinationEndDate   = DateOnly.FromDateTime(td.EndDate),
                        DestinationName      = destDoc.Name,
                        DestinationState     = destDoc.State,
                        DestinationCountry   = destDoc.Country,
                        MaxBuddies           = trip.MaxBuddies ?? 0,
                        AcceptedBuddiesCount = acceptedCount
                    });
                }

                result.Add(overview);
            }

            return result;
        }



        // ---------------------------------------------------------
        // Get owner of a specific trip destination
        // ---------------------------------------------------------
        public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
        {
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == tripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            return trip?.OwnerId;
        }

        // ---------------------------------------------------------
        // Create a new trip with destinations
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(
        CreateTripWithDestinationsDto dto)
        {
            try
            {
                //Load existing trips to find next IDs
                var trips = await LoadTripsAsync();

                var nextTripId = trips
                    .DefaultIfEmpty()
                    .Max(t => t?.TripId ?? 0) + 1;

                var nextTripDestinationId = trips
                    .SelectMany(t => t.Destinations)
                    .DefaultIfEmpty()
                    .Max(td => td?.TripDestinationId ?? 0) + 1;

                //Trip-level data from dto.CreateTrip
                var t = dto.CreateTrip;

                var tripDoc = new TripDocument
                {
                    TripId      = nextTripId,
                    OwnerId     = t.OwnerId,
                    MaxBuddies  = t.MaxBuddies,
                    StartDate   = t.StartDate,
                    EndDate     = t.EndDate,
                    Description = t.Description,
                    IsArchived  = false,
                    Destinations = new List<TripDestinationEmbedded>()
                };

                //Preload destinations (if needed for ID generation)
                var existingDestinations = await _destinationsCollection
                    .Find(FilterDefinition<DestinationDocument>.Empty)
                    .ToListAsync();

                var nextDestinationId = existingDestinations
                    .DefaultIfEmpty()
                    .Max(d => d?.DestinationId ?? 0) + 1;

                //Map trip destinations
                int seq = 1;
                foreach (var d in dto.TripDestinations)
                {
                    int destinationId;

                    if (d.DestinationId.HasValue)
                    {
                        destinationId = d.DestinationId.Value;
                    }
                    else
                    {
                        // Create new destination entry
                        var newDest = new DestinationDocument
                        {
                            DestinationId = nextDestinationId++,
                            Name          = d.Name ?? "Unnamed",
                            State         = d.State,
                            Country       = d.Country ?? string.Empty,
                            Longitude     = d.Longitude,
                            Latitude      = d.Latitude
                        };

                        await _destinationsCollection.InsertOneAsync(newDest);
                        destinationId = newDest.DestinationId;
                    }

                    var td = new TripDestinationEmbedded
                    {
                        TripDestinationId = nextTripDestinationId++,
                        DestinationId     = destinationId,
                        StartDate         = d.DestinationStartDate.ToDateTime(TimeOnly.MinValue), // ✅ DateOnly → DateTime
                        EndDate           = d.DestinationEndDate.ToDateTime(TimeOnly.MinValue),   // ✅
                        SequenceNumber    = d.SequenceNumber != 0 ? d.SequenceNumber : seq++,
                        Description       = d.Description,
                        IsArchived        = false,
                        Buddies           = new List<BuddyEmbedded>()
                    };

                    tripDoc.Destinations.Add(td);
                }

                //Insert into Mongo
                await _tripsCollection.InsertOneAsync(tripDoc);

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, $"Error creating trip: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // Leave Trip destination
        // ---------------------------------------------------------

        public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
        int userId, int tripDestinationId, int personCount, string reason)
        {
            //Find the trip that contains this trip destination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == tripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return (false, "Trip not found for given destination ID.");

            //Find the trip destination within the trip
            var destination = trip.Destinations.FirstOrDefault(td => td.TripDestinationId == tripDestinationId);
            if (destination == null)
                return (false, "Trip destination not found.");

            //Find the buddy entry for this user
            var buddy = destination.Buddies.FirstOrDefault(b => b.UserId == userId);
            if (buddy == null)
                return (false, "Buddy not found for this user in the trip destination.");

            //Adjust person count or remove buddy
            if (buddy.PersonCount.HasValue && buddy.PersonCount.Value > personCount)
            {
                buddy.PersonCount -= personCount;
            }
            else
            {
                // Remove the buddy entirely
                destination.Buddies.Remove(buddy);
            }

            // Optionally: you could add an audit entry or mark a "LeftReason"
            // e.g. buddy.Note = $"Left: {reason}";

            //Replace the updated destination back into the trip
            var destinationIndex = trip.Destinations.FindIndex(td => td.TripDestinationId == tripDestinationId);
            if (destinationIndex >= 0)
                trip.Destinations[destinationIndex] = destination;

            //Save the updated trip back to MongoDB
            var updateFilter = Builders<TripDocument>.Filter.Eq(t => t.TripId, trip.TripId);
            var result = await _tripsCollection.ReplaceOneAsync(updateFilter, trip);

            if (result.ModifiedCount == 0)
                return (false, "Failed to update trip record.");

            return (true, null);
        }

        // ---------------------------------------------------------
        // Pending buddy requests for a trip owner
        // ---------------------------------------------------------
        public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
        {
            var trips = await LoadTripsAsync();

            // Only trips where this user is the owner
            var ownedTrips = trips.Where(t => t.OwnerId == userId).ToList();

            if (!ownedTrips.Any())
                return Enumerable.Empty<PendingBuddyRequest>();

            // Build lookups for destinations and users
            var destIds = ownedTrips
                .SelectMany(t => t.Destinations)
                .Select(td => td.DestinationId)
                .Distinct()
                .ToList();

            var destinationDocs = await _destinationsCollection
                .Find(d => destIds.Contains(d.DestinationId))
                .ToListAsync();

            var destMap = destinationDocs.ToDictionary(d => d.DestinationId, d => d);

            var userIds = ownedTrips
                .SelectMany(t => t.Destinations)
                .SelectMany(td => td.Buddies)
                .Where(b => string.Equals(b.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase))
                .Select(b => b.UserId)
                .Distinct()
                .ToList();

            var userDocs = await _usersCollection
                .Find(u => userIds.Contains(u.UserId))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.UserId, u => u);

            var result = new List<PendingBuddyRequest>();

            foreach (var trip in ownedTrips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    foreach (var b in td.Buddies.Where(b =>
                        string.Equals(b.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase)))
                    {
                        userMap.TryGetValue(b.UserId, out var uDoc);

                        result.Add(new PendingBuddyRequest
                        {
                            TripId = trip.TripId,
                            DestinationName = destDoc.Name,
                            BuddyId = b.BuddyId,
                            RequesterUserId = b.UserId,
                            RequesterName = uDoc?.Name ?? string.Empty,
                            BuddyNote = b.Note,
                            PersonCount = b.PersonCount ?? 1
                        });
                    }
                }
            }

            return result;
        }

        // ---------------------------------------------------------
        // User sends a new buddy request
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            // Validate input parameters (matching MySQL stored procedure validation)
            if (buddyDto.UserId <= 0)
                return (false, "user_id cannot be NULL");

            if (buddyDto.TripDestinationId <= 0)
                return (false, "trip_destination_id cannot be NULL");

            if (buddyDto.PersonCount <= 0)
                return (false, "person_count must be greater than zero");

            // Find the trip containing this TripDestination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == buddyDto.TripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return (false, "Trip destination not found");

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == buddyDto.TripDestinationId);
            if (td == null)
                return (false, "Trip destination not found");

            // Check if user already has a request for this destination
            var existing = td.Buddies.FirstOrDefault(b => b.UserId == buddyDto.UserId);
            if (existing != null)
                return (false, "User has already requested to join this trip destination");

            // Calculate remaining capacity (matching MySQL get_trip_destination_remaining_capacity)
            var maxBuddies = trip.MaxBuddies ?? 0;
            var acceptedPersons = td.Buddies
                .Where(b => string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                .Sum(b => b.PersonCount ?? 1);

            var remainingCapacity = maxBuddies > 0 ? Math.Max(0, maxBuddies - acceptedPersons) : 0;

            // Check if there's enough capacity
            if (remainingCapacity < buddyDto.PersonCount)
                return (false, "there is not enough buddy capacity for the person_count");

            var newBuddyId = await GetNextBuddyIdAsync();

            var buddy = new BuddyEmbedded
            {
                BuddyId = newBuddyId,
                UserId = buddyDto.UserId,
                PersonCount = buddyDto.PersonCount,
                Note = buddyDto.Note,
                IsActive = false,
                DepartureReason = null,
                RequestStatus = "pending"
            };

            td.Buddies.Add(buddy);

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == trip.TripId,
                trip);

            return (replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0, null);
        }

        // ---------------------------------------------------------
        // Owner accepts/rejects a buddy request
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            var trips = await LoadTripsAsync();

            TripDocument? tripWithBuddy = null;
            TripDestinationEmbedded? tdWithBuddy = null;
            BuddyEmbedded? buddy = null;

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    var b = td.Buddies.FirstOrDefault(x => x.BuddyId == updateBuddyRequestDto.BuddyId);
                    if (b != null)
                    {
                        tripWithBuddy = trip;
                        tdWithBuddy = td;
                        buddy = b;
                        break;
                    }
                }
                if (buddy != null) break;
            }

            if (tripWithBuddy == null || tdWithBuddy == null || buddy == null)
                return (false, "Buddy request not found");

            var newStatus = updateBuddyRequestDto.NewStatus switch
            {
                BuddyRequestUpdateStatus.Accepted => "accepted",
                BuddyRequestUpdateStatus.Rejected => "rejected",
                _ => "pending"
            };

            buddy.RequestStatus = newStatus;

            if (newStatus == "accepted")
            {
                buddy.IsActive = true;
                buddy.DepartureReason = null;
            }
            else if (newStatus == "rejected")
            {
                buddy.IsActive = false;
            }

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == tripWithBuddy.TripId,
                tripWithBuddy);

            return (replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0, null);
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<TripAudit>> GetTripAuditsAsync()
        {
            var database = _tripsCollection.Database;
            var tripAuditCollection = database.GetCollection<TripAuditDocument>("trip_audits");
            
            var docs = await tripAuditCollection
                .Find(FilterDefinition<TripAuditDocument>.Empty)
                .ToListAsync();
            
            return docs.Select(doc => new TripAudit
            {
                AuditId = doc.AuditId,
                TripId = doc.TripId,
                Action = doc.Action,
                FieldChanged = doc.FieldChanged,
                OldValue = doc.OldValue,
                NewValue = doc.NewValue,
                ChangedBy = doc.ChangedBy,
                Timestamp = doc.Timestamp
            }).ToList();
        }
        public async Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync()
        {
            var database = _tripsCollection.Database;
            var buddyAuditCollection = database.GetCollection<BuddyAuditDocument>("buddy_audits");
            
            var docs = await buddyAuditCollection
                .Find(FilterDefinition<BuddyAuditDocument>.Empty)
                .ToListAsync();
            
            return docs.Select(doc => new BuddyAudit
            {
                AuditId = doc.AuditId,
                BuddyId = doc.BuddyId,
                Action = doc.Action,
                Reason = doc.Reason,
                ChangedBy = doc.ChangedBy,
                Timestamp = doc.Timestamp
            }).ToList();
        }
    }
}