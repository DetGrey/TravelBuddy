using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips;
// INTERFACE
public interface IBuddyRepository
{
    Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId);
    Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto);
    Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto);
}