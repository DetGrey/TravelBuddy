using TravelBuddy.Trips; 

namespace TravelBuddy.Api.Factories;
/// Implements the Factory pattern to dynamically select a concrete ITripRepository
/// based on the 'X-Database-Type' HTTP request header.
public class TripRepositoryFactory : ITripRepositoryFactory
{
    // Used to access the current HTTP request (including headers).
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Used to manually resolve the transient concrete repository class.
    private readonly IServiceProvider _serviceProvider;
    
    // Stores the resolved database type to avoid reading the header multiple times per request.
    private readonly string _dbType; 

    public TripRepositoryFactory(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;

        // Resolve the DB type once in the constructor. This runs per-request (Scoped lifetime).
        _dbType = _httpContextAccessor.HttpContext
                            ?.Request.Headers["X-Database-Type"]
                            .FirstOrDefault()
                            ?.ToLowerInvariant() ?? "mysql"; // Defaults to MySQL if header is missing
    }

    public ITripDestinationRepository GetTripDestinationRepository()
    {
        return _dbType switch
        {
            // Note: We use GetRequiredService<T>() here to resolve the concrete Transient registrations
            "mongodb" => _serviceProvider.GetRequiredService<MongoDbTripDestinationRepository>(),
            "neo4j" => _serviceProvider.GetRequiredService<Neo4jTripDestinationRepository>(),
            _ => _serviceProvider.GetRequiredService<MySqlTripDestinationRepository>() // Defaults to MySQL
        };
    }
    public IBuddyRepository GetBuddyRepository()
    {
        return _dbType switch
        {
            // Note: We use GetRequiredService<T>() here to resolve the concrete Transient registrations
            "mongodb" => _serviceProvider.GetRequiredService<MongoDbBuddyRepository>(),
            "neo4j" => _serviceProvider.GetRequiredService<Neo4jBuddyRepository>(),
            _ => _serviceProvider.GetRequiredService<MySqlBuddyRepository>() // Defaults to MySQL
        };
    }
}