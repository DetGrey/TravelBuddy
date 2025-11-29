using Microsoft.EntityFrameworkCore;
using MySqlConnector;
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

    public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        try
        {
            var noteParam = new MySqlParameter { 
                MySqlDbType = MySqlDbType.VarChar, 
                Size = 255, 
                Value = (object?)buddyDto.Note ?? DBNull.Value 
            };

            await _context.Database.ExecuteSqlRawAsync(
                "CALL request_to_join_trip_destination({0}, {1}, {2}, {3})",
                buddyDto.UserId,
                buddyDto.TripDestinationId,
                buddyDto.PersonCount,
                noteParam
            );

            return (true, null);
        }
        catch (Exception ex)
        {
            // If it's a MySqlException, get the real message
            if (ex is MySqlException mysqlEx)
            {
                return (false, "Error: " + mysqlEx.Message);
            }

            // Otherwise fall back to generic
            return (false, "Error: " + ex.Message);
        }
    }
    public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_buddy_request_tx(
                    {updateBuddyRequestDto.BuddyId},
                    {updateBuddyRequestDto.UserId},
                    {updateBuddyRequestDto.NewStatus.ToString().ToLower()}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
             return (false, "Error: " + ex.Message); 
        }
    }
}