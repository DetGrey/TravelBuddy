using TravelBuddy.SharedKernel;
using TravelBuddy.Trips; 

namespace TravelBuddy.Api.Factories;

public class SharedKernelRepositoryFactory : ISharedKernelRepositoryFactory
{
    // Used to access the current HTTP request (including headers).
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Used to manually resolve the transient concrete repository class.
    private readonly IServiceProvider _serviceProvider;
    
    // Stores the resolved database type to avoid reading the header multiple times per request.
    private readonly string _dbType; 

    public SharedKernelRepositoryFactory(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;

        // Resolve the DB type once in the constructor. This runs per-request (Scoped lifetime).
        _dbType = _httpContextAccessor.HttpContext
                            ?.Request.Headers["X-Database-Type"]
                            .FirstOrDefault()
                            ?.ToLowerInvariant() ?? "mysql"; // Defaults to MySQL if header is missing
    }

    public ISharedKernelRepository GetSharedKernelRepository()
    {
        return _dbType switch
        {
            // Note: We use GetRequiredService<T>() here to resolve the concrete Transient registrations
            "mongodb" => _serviceProvider.GetRequiredService<MongoDbSharedKernelRepository>(),
            "neo4j" => _serviceProvider.GetRequiredService<Neo4jSharedKernelRepository>(),
            _ => _serviceProvider.GetRequiredService<MySqlSharedKernelRepository>() // Defaults to MySQL
        };
    }
}