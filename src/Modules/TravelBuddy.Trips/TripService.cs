using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Models;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface ITripService
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
        Task<IEnumerable<DestinationDto>> GetDestinationsAsync();
        Task<IEnumerable<BuddyTripSummaryDto>> GetBuddyTripsAsync(int userId);
        Task<TripDestinationInfoDto?> GetTripDestinationInfoAsync(int tripDestinationId);
        Task<TripOverviewDto?> GetFullTripOverviewAsync(int tripId);
        Task<IEnumerable<TripOverviewDto>> GetOwnedTripOverviewsAsync(int userId);
        Task<bool> IsTripOwnerAsync(int userId, int tripDestinationId);
        Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto);
        
        // Buddy related
        Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int triggeredBy, string departureReason);
        Task<IEnumerable<PendingBuddyRequestDto>> GetPendingBuddyRequestsAsync(int userId);
        Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto);
        Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto);

        // Audit related
        Task<IEnumerable<TripAuditDto>> GetTripAuditsAsync();
        Task<IEnumerable<BuddyAuditDto>> GetBuddyAuditsAsync();
   }

    // CLASS
    public class TripService : ITripService
    {
        private readonly ITripRepositoryFactory _tripRepositoryFactory;

        public TripService(ITripRepositoryFactory tripRepositoryFactory)
        {
            _tripRepositoryFactory = tripRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private ITripRepository GetRepo() => _tripRepositoryFactory.GetTripRepository();

        public async Task<IEnumerable<TripDestinationSearchDto>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var tripRepository = GetRepo();
            var results = await tripRepository.SearchTripsAsync(reqStart, reqEnd, country, state, name, partySize, q);

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
        public async Task<IEnumerable<DestinationDto>> GetDestinationsAsync()
        {
            var tripRepository = GetRepo();
            var results = await tripRepository.GetDestinationsAsync();

            return results.Select(r => new DestinationDto(
                r.DestinationId,
                r.Name,
                r.State,
                r.Country,
                r.Longitude,
                r.Latitude
            )).ToList();
        }
        public async Task<IEnumerable<BuddyTripSummaryDto>> GetBuddyTripsAsync(int userId)
        {
            var tripRepository = GetRepo();
            var results = await tripRepository.GetBuddyTripsAsync(userId);

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
            var tripRepository = GetRepo();
            var result = await tripRepository.GetTripDestinationInfoAsync(tripDestinationId);

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
            var tripRepository = GetRepo();
            var tripOverview = await tripRepository.GetFullTripOverviewAsync(tripId);
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
            var tripRepository = GetRepo();
            var tripOverviews = await tripRepository.GetOwnedTripOverviewsAsync(userId);

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
            var tripRepository = GetRepo();
            var tripOwner = await tripRepository.GetTripOwnerAsync(tripDestinationId);
            return tripOwner == userId;
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto)
        {
            var tripRepository = GetRepo();
            return await tripRepository.CreateTripWithDestinationsAsync(createTripWithDestinationsDto);
        }

        // --------------------------------------------------------------------------
        // BUDDY RELATED METHODS
        // --------------------------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
            int userId,
            int tripDestinationId,
            int triggeredBy, 
            string departureReason
        ) {
            var tripRepository = GetRepo();
            return await tripRepository.LeaveTripDestinationAsync(userId, tripDestinationId, triggeredBy, departureReason);
        }
        public async Task<IEnumerable<PendingBuddyRequestDto>> GetPendingBuddyRequestsAsync(int userId)
        {
            var buddyRepository = GetRepo();

            var results = await buddyRepository.GetPendingBuddyRequestsAsync(userId);

            return results.Select(r => new PendingBuddyRequestDto(
                r.TripDestinationId,
                r.DestinationName,
                r.DestinationStartDate,
                r.DestinationEndDate,
                r.TripId,
                r.BuddyId,
                r.RequesterUserId,
                r.RequesterName,
                r.BuddyNote,
                r.PersonCount
            )).ToList();
        }

        public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            var buddyRepository = GetRepo();
            return await buddyRepository.InsertBuddyRequestAsync(buddyDto);
        }

        public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            var buddyRepository = GetRepo();
            return await buddyRepository.UpdateBuddyRequestAsync(updateBuddyRequestDto);
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<TripAuditDto>> GetTripAuditsAsync()
        {
            var tripRepository = GetRepo();
            var results = await tripRepository.GetTripAuditsAsync();
            return results.Select(ta => new TripAuditDto(
                ta.AuditId,
                ta.TripId,
                ta.Action,
                ta.FieldChanged,
                ta.OldValue,
                ta.NewValue,
                ta.ChangedBy,
                ta.Timestamp
            )).ToList();
        }
        public async Task<IEnumerable<BuddyAuditDto>> GetBuddyAuditsAsync()
        {
            var tripRepository = GetRepo();
            var results = await tripRepository.GetBuddyAuditsAsync();
            return results.Select(ba => new BuddyAuditDto(
                ba.AuditId,
                ba.BuddyId,
                ba.Action,
                ba.Reason,
                ba.ChangedBy,
                ba.Timestamp
            )).ToList();
        }
    }
}