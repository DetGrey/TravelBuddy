using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips;
// CLASS
public class MySqlBuddyRepository : IBuddyRepository
{
    private readonly TripsDbContext _context;

    public MySqlBuddyRepository(TripsDbContext context)
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

    public async Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL request_to_join_trip_destination(
                {buddyDto.UserId}, 
                {buddyDto.TripDestinationId},
                {buddyDto.PersonCount},
                {buddyDto.Note}
            )");

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_buddy_request_tx(
                    {updateBuddyRequestDto.BuddyId},
                    {updateBuddyRequestDto.UserId},
                    {updateBuddyRequestDto.NewStatus.ToString().ToLower()}
                )");

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}