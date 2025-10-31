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
                    CALL get_user_trips({userId})")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}