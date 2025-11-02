using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface IBuddyService
    {
        Task<IEnumerable<PendingBuddyRequestsDto>> GetPendingBuddyRequestsAsync(int userId);
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
                r.BuddyUserId,
                r.BuddyName,
                r.BuddyNote,
                r.PersonCount
            )).ToList();
        }
    }
}