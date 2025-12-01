using Microsoft.EntityFrameworkCore;
using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel;
public interface ISharedKernelRepository
{
    Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync();
}
