using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface ITripDestinationRepository
    {
        Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q
        );

        Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId);
        Task<int?> GetTripOwnerAsync(int tripDestinationId);
        Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int triggeredBy, string departureReason);
    }

    // CLASS
    public class TripDestinationRepository : ITripDestinationRepository
    {
        private readonly TripsDbContext _context;

        public TripDestinationRepository(TripsDbContext context)
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
}