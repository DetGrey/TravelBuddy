using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Models;

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

        Task<IEnumerable<BuddyTripSummaryDto>> GetBuddyTripsAsync(int userId);
        Task<TripDestinationInfoDto?> GetTripDestinationInfoAsync(int tripDestinationId);
        Task<TripOverviewDto?> GetFullTripOverviewAsync(int tripId);
        Task<IEnumerable<TripOverviewDto>> GetOwnedTripOverviewsAsync(int userId);
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

        public async Task<IEnumerable<BuddyTripSummaryDto>> GetBuddyTripsAsync(int userId)
        {
            var tripDestinationRepository = GetRepo();
            var results = await tripDestinationRepository.GetBuddyTripsAsync(userId);

            return results.Select(r => new BuddyTripSummaryDto(
                r.TripId,
                r.TripDestinationId,
                r.DestinationName,
                r.TripDescription,
                r.StartDate,
                r.EndDate,
                r.IsArchived
            )).ToList();
        }
        public async Task<TripDestinationInfoDto?> GetTripDestinationInfoAsync(int tripDestinationId)
        {
            var tripDestinationRepository = GetRepo();
            var result = await tripDestinationRepository.GetTripDestinationInfoAsync(tripDestinationId);

            if (result == null)
            {
                return null;
            }

            return new TripDestinationInfoDto(
                result.TripDestinationId,
                result.DestinationStartDate,
                result.DestinationEndDate,
                result.DestinationDescription,
                result.DestinationIsArchived,
                result.TripId,
                result.MaxBuddies,
                result.DestinationId,
                result.DestinationName,
                result.DestinationState,
                result.DestinationCountry,
                result.Longitude,
                result.Latitude,
                result.OwnerUserId,
                result.OwnerName,
                result.GroupConversationId,
                result.AcceptedBuddies.Select(b => new BuddyInfoDto(
                    b.BuddyId,
                    b.PersonCount,
                    b.BuddyNote,
                    b.BuddyUserId,
                    b.BuddyName
                )),
                result.PendingRequests.Select(r => new BuddyRequestInfoDto(
                    r.BuddyId,
                    r.PersonCount,
                    r.BuddyNote,
                    r.RequesterUserId,
                    r.RequesterName
                ))
            );
        }
        public async Task<TripOverviewDto?> GetFullTripOverviewAsync(int tripId)
        {
            var tripDestinationRepository = GetRepo();
            var tripOverview = await tripDestinationRepository.GetFullTripOverviewAsync(tripId);
            if (tripOverview == null)
            {
                return null;
            }
            return new TripOverviewDto(
                tripOverview.TripId,
                tripOverview.TripName,
                tripOverview.TripStartDate,
                tripOverview.TripEndDate,
                tripOverview.MaxBuddies,
                tripOverview.TripDescription,
                tripOverview.OwnerUserId,
                tripOverview.OwnerName,
                tripOverview.Destinations.Select(d => new SimplifiedTripDestinationDto(
                    d.TripDestinationId,
                    d.TripId,
                    d.DestinationStartDate,
                    d.DestinationEndDate,
                    d.DestinationName,
                    d.DestinationState,
                    d.DestinationCountry,
                    d.MaxBuddies,
                    d.AcceptedBuddiesCount
                )).ToList()
            );
        }
        public async Task<IEnumerable<TripOverviewDto>> GetOwnedTripOverviewsAsync(int userId)
        {
            var tripDestinationRepository = GetRepo();
            var tripOverviews = await tripDestinationRepository.GetOwnedTripOverviewsAsync(userId);

            return tripOverviews.Select(tripOverview => new TripOverviewDto(
                tripOverview.TripId,
                tripOverview.TripName,
                tripOverview.TripStartDate,
                tripOverview.TripEndDate,
                tripOverview.MaxBuddies,
                tripOverview.TripDescription,
                tripOverview.OwnerUserId,
                tripOverview.OwnerName,
                tripOverview.Destinations.Select(d => new SimplifiedTripDestinationDto(
                    d.TripDestinationId,
                    d.TripId,
                    d.DestinationStartDate,
                    d.DestinationEndDate,
                    d.DestinationName,
                    d.DestinationState,
                    d.DestinationCountry,
                    d.MaxBuddies,
                    d.AcceptedBuddiesCount
                )).ToList()
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