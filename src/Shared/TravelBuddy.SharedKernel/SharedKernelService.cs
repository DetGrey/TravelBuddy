using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel
{
    // INTERFACE
    public interface ISharedKernelService
    {
        Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync();
   }

    // CLASS
    public class SharedKernelService : ISharedKernelService
    {
        private readonly ISharedKernelRepositoryFactory _sharedKernelRepositoryFactory;

        public SharedKernelService(ISharedKernelRepositoryFactory sharedKernelRepositoryFactory)
        {
            _sharedKernelRepositoryFactory = sharedKernelRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private ISharedKernelRepository GetRepo() => _sharedKernelRepositoryFactory.GetSharedKernelRepository();
        
        public async Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync()
        {
            return await GetRepo().GetSystemEventLogsAsync();
        }
    }
}