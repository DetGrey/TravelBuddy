using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface IBuddyService
    {
        Task<IEnumerable<PendingBuddyRequestsDto>> GetPendingBuddyRequestsAsync(int userId);
        Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto);
        Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto);
    }

    // CLASS
    public class BuddyService : IBuddyService
    {
        private readonly ITripRepositoryFactory _tripRepositoryFactory;

        public BuddyService(ITripRepositoryFactory tripRepositoryFactory)
        {
            _tripRepositoryFactory = tripRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private IBuddyRepository GetRepo() => _tripRepositoryFactory.GetBuddyRepository();

        public async Task<IEnumerable<PendingBuddyRequestsDto>> GetPendingBuddyRequestsAsync(int userId)
        {
            var buddyRepository = GetRepo();

            var results = await buddyRepository.GetPendingBuddyRequestsAsync(userId);

            return results.Select(r => new PendingBuddyRequestsDto(
                r.TripId,
                r.DestinationName,
                r.BuddyId,
                r.UserId,
                r.BuddyName,
                r.BuddyNote,
                r.PersonCount
            )).ToList();
        }

        public async Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            var buddyRepository = GetRepo();
            return await buddyRepository.InsertBuddyRequestAsync(buddyDto);
        }

        public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            var buddyRepository = GetRepo();
            return await buddyRepository.UpdateBuddyRequestAsync(updateBuddyRequestDto);
        }
    }
}