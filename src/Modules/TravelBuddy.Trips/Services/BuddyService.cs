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
        private readonly IBuddyRepository _repository;

        public BuddyService(IBuddyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PendingBuddyRequestsDto>> GetPendingBuddyRequestsAsync(int userId)
        {
            var results = await _repository.GetPendingBuddyRequestsAsync(userId);

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
            return await _repository.InsertBuddyRequestAsync(buddyDto);
        }

        public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            return await _repository.UpdateBuddyRequestAsync(updateBuddyRequestDto);
        }
    }
}