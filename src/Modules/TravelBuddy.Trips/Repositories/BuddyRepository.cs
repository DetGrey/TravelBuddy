using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips
{
    // INTERFACE
    public interface IBuddyRepository
    {
        Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId);
    }

    // CLASS
    public class BuddyRepository : IBuddyRepository
    {
        private readonly TripsDbContext _context;

        public BuddyRepository(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
        {
            return await _context.PendingBuddyRequests
                .FromSqlInterpolated($@"
                    CALL get_pending_buddy_requests({userId})")
                .AsNoTracking()
                .ToListAsync();
        }
    }
}