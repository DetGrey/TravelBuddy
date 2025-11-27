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
        Task<bool> IsTripOwnerAsync(int userId, int tripDestinationId);
        Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int triggeredBy, string departureReason);
    }

    // CLASS
    public class TripDestinationService : ITripDestinationService
    {
        private readonly ITripRepositoryFactory _tripRepositoryFactory;

        public TripDestinationService(ITripRepositoryFactory tripRepositoryFactory)
        {
            _tripRepositoryFactory = tripRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private ITripDestinationRepository GetRepo() => _tripRepositoryFactory.GetTripDestinationRepository();

        public async Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var tripDestinationRepository = GetRepo();
            var results = await tripDestinationRepository.SearchTripsAsync(reqStart, reqEnd, country, state, name, partySize, q);

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
            var tripDestinationRepository = GetRepo();
            var results = await tripDestinationRepository.GetUserTripsAsync(userId);

            return results.Select(r => new UserTripSummaryDto(
                r.TripId,
                r.TripDestinationId,
                r.DestinationName,
                r.TripDescription,
                r.Role
            )).ToList();
        }
        public async Task<bool> IsTripOwnerAsync(int userId, int tripDestinationId)
        {
            var tripDestinationRepository = GetRepo();
            var tripOwner = await tripDestinationRepository.GetTripOwnerAsync(tripDestinationId);
            return tripOwner == userId;
        }
        public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
            int userId,
            int tripDestinationId,
            int triggeredBy, 
            string departureReason
        ) {
            var tripDestinationRepository = GetRepo();
            return await tripDestinationRepository.LeaveTripDestinationAsync(userId, tripDestinationId, triggeredBy, departureReason);
        }
    }
}