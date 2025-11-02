using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface ITripDestinationService
    {
        Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q
        );

        Task<IEnumerable<UserTripSummaryDto>> GetUserTripsAsync(int userId);
    }

    // CLASS
    public class TripDestinationService : ITripDestinationService
    {
        private readonly ITripDestinationRepository _repository;

        public TripDestinationService(ITripDestinationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var results = await _repository.SearchTripsAsync(reqStart, reqEnd, country, state, name, partySize, q);

            return results.Select(r => new TripDestinationSearchDto(
              r.TripDestinationId,
              r.TripId,
              r.DestinationId,
              r.DestinationName,
              r.Country,
              r.State,
              r.DestinationStart,
              r.DestinationEnd,
              r.MaxBuddies,
              r.AcceptedPersons,
              r.RemainingCapacity
          )).ToList();

        }

        public async Task<IEnumerable<UserTripSummaryDto>> GetUserTripsAsync(int userId)
        {
            var results = await _repository.GetUserTripsAsync(userId);

            return results.Select(r => new UserTripSummaryDto(
                r.TripId,
                r.TripDestinationId,
                r.DestinationName,
                r.TripDescription,
                r.Role
            )).ToList();
        }
    }
}