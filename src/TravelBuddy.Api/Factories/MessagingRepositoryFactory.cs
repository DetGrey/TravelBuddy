using TravelBuddy.Messaging;

namespace TravelBuddy.Api.Factories;
/// Implements the Factory pattern to dynamically select a concrete ITripRepository
/// based on the 'X-Database-Type' HTTP request header.
public class MessagingRepositoryFactory : IMessagingRepositoryFactory
{
    // Used to access the current HTTP request (including headers).
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Used to manually resolve the transient concrete repository class.
    private readonly IServiceProvider _serviceProvider;
    
    // Stores the resolved database type to avoid reading the header multiple times per request.
    private readonly string _dbType; 

    public MessagingRepositoryFactory(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;

        // Resolve the DB type once in the constructor. This runs per-request (Scoped lifetime).
        _dbType = _httpContextAccessor.HttpContext
                            ?.Request.Headers["X-Database-Type"]
                            .FirstOrDefault()
                            ?.ToLowerInvariant() ?? "mysql"; // Defaults to MySQL if header is missing
    }

    public IMessagingRepository GetMessagingRepository()
    {
        return _dbType switch
        {
            // Note: We use GetRequiredService<T>() here to resolve the concrete Transient registrations
            "mongodb" => _serviceProvider.GetRequiredService<MongoDbMessagingRepository>(),
            "neo4j" => _serviceProvider.GetRequiredService<Neo4jMessagingRepository>(),
            _ => _serviceProvider.GetRequiredService<MySqlMessagingRepository>() // Defaults to MySQL
        };
    }
}