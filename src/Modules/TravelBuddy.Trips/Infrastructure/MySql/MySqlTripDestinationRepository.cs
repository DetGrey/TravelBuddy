using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
// CLASS
public class MySqlTripDestinationRepository : ITripDestinationRepository
{
    private readonly TripsDbContext _context;

    public MySqlTripDestinationRepository(TripsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
        DateOnly? reqStart,
        DateOnly? reqEnd,
        string? country,
        string? state,
        string? name,
        int? partySize,
        string? q)
    {
        var start = reqStart.HasValue ? reqStart.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
        var end = reqEnd.HasValue ? reqEnd.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
        var countryParam = country ?? string.Empty;
        var stateParam = state ?? string.Empty;
        var nameParam = name ?? string.Empty;
        var partySizeParam = partySize ?? 0;
        var qParam = q ?? string.Empty;

        return await _context.Set<TripDestinationSearchResult>()
            .FromSqlInterpolated($@"
                CALL search_trips(
                    {start},
                    {end},
                    {countryParam},
                    {stateParam},
                    {nameParam},
                    {partySizeParam},
                    {qParam}
                )")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId)
    {
        return await _context.UserTripSummaries
            .FromSqlInterpolated($@"
                CALL get_user_trips({userId})") // FromSqlInterpolated() automatically prevents SQL injections
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
    {
        // Ensure TripDestinationInfo is configured to map to V_TripDestinationInfo
        // Use standard LINQ to apply the WHERE clause asynchronously.
        var tripDestination = await _context.TripDestinationInfo
            .FromSqlInterpolated($"SELECT * FROM V_TripDestinationInfo WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (tripDestination == null) return null;

        // Load Accepted Buddies
        // Ensure BuddyInfo is configured to map to V_AcceptedBuddies
        var acceptedBuddies = await _context.BuddyInfo
            .FromSqlInterpolated($"SELECT * FROM V_AcceptedBuddies WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .ToListAsync();

        // Load Pending Requests
        // Ensure BuddyRequestInfo is configured to map to V_PendingRequests
        var pendingRequests = await _context.BuddyRequestInfo
            .FromSqlInterpolated($"SELECT * FROM V_PendingRequests WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .ToListAsync();
            
        // Populate the lists
        tripDestination.AcceptedBuddies.AddRange(acceptedBuddies);
        tripDestination.PendingRequests.AddRange(pendingRequests);

        return tripDestination;
    }
    private async Task<List<SimplifiedTripDestination>> GetTripDestinationsWithCountsAsync(int tripId)
    {
        var destinations = await _context.SimplifiedTripDestination
            .FromSqlInterpolated($"SELECT * FROM V_SimplifiedTripDest WHERE TripId = {tripId}")
            .AsNoTracking()
            .ToListAsync();

        if (!destinations.Any())
        {
            return new List<SimplifiedTripDestination>();
        }

        var destinationIds = destinations.Select(d => d.TripDestinationId).ToList();

        var buddyCounts = await _context.Buddies
            .Where(b => destinationIds.Contains(b.TripDestinationId) &&
                        b.RequestStatus == "accepted" &&
                        b.IsActive == true)
            .GroupBy(b => b.TripDestinationId)
            .Select(g => new
            {
                TripDestinationId = g.Key,
                AcceptedCount = g.Sum(b => b.PersonCount)
            })
            .ToListAsync();
            
        var countsDictionary = buddyCounts.ToDictionary(x => x.TripDestinationId, x => x.AcceptedCount);

        foreach (var dest in destinations)
        {
            if (countsDictionary.TryGetValue(dest.TripDestinationId, out var count))
            {
                dest.AcceptedBuddiesCount = count ?? 0;
            }
            else
            {
                dest.AcceptedBuddiesCount = 0;
            }
        }

        return destinations;
    }
    public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
    {
        var tripHeader = await _context.TripHeaderInfo
            .FromSqlInterpolated($"SELECT * FROM V_TripHeaderInfo WHERE TripId = {tripId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (tripHeader == null) return null;
        
        var destinationsWithCounts = await GetTripDestinationsWithCountsAsync(tripId);

        var tripOverview = new TripOverview
        {
            // Map Header Properties
            TripId = tripHeader.TripId,
            TripName = tripHeader.TripName,
            TripStartDate = tripHeader.TripStartDate,
            TripEndDate = tripHeader.TripEndDate,
            MaxBuddies = tripHeader.MaxBuddies,
            TripDescription = tripHeader.TripDescription,
            OwnerUserId = tripHeader.OwnerUserId,
            OwnerName = tripHeader.OwnerName,
            
            // Assign the enriched destination list
            Destinations = destinationsWithCounts
        };

        return tripOverview;
    }
    public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
    {
        // SqlQuery is better than FromSqlInterpolated when it returns only one value
        return await _context.Database
            .SqlQuery<int>($@"
                SELECT trip.owner_id AS Value
                FROM trip
                JOIN travel_buddy.trip_destination td ON trip.trip_id = td.trip_id
                WHERE td.trip_destination_id = {tripDestinationId}
                LIMIT 1")
            .SingleOrDefaultAsync();
    }
    public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
        int userId,
        int tripDestinationId,
        int triggeredBy,
        string departureReason
    )
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL remove_buddy_from_trip_destination(
                    {userId},
                    {tripDestinationId},
                    {triggeredBy},
                    {departureReason}
                )");

            return (true, null); // Success
        }
        catch (Exception ex)
        {
            // For general debugging, you can return the full exception message
            // which usually contains the database error details.
            // If you're using a specific provider (like Npgsql for PostgreSQL 
            // or SqlClient for SQL Server), you might need to cast 'ex' 
            // to get the specific database exception type (e.g., PostgresException) 
            // for more granular error codes/messages.

            return (false, "Error: " + ex.Message); // Failure with error message
        }
    }
}
