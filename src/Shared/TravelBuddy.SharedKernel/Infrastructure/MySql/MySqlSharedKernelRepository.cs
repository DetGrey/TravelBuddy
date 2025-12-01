using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TravelBuddy.SharedKernel.Infrastructure;
using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel;
// CLASS
public class MySqlSharedKernelRepository : ISharedKernelRepository
{
    private readonly SharedKernelDbContext _context;

    public MySqlSharedKernelRepository(SharedKernelDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync()
    {
        return await _context.SystemEventLogs
            .AsNoTracking()
            .ToListAsync();
    }
}
