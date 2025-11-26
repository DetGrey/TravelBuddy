using TravelBuddy.Users;

namespace TravelBuddy.Api.Factories;
/// Implements the Factory pattern to dynamically select a concrete IUserRepository
/// based on the 'X-Database-Type' HTTP request header.
public class UserRepositoryFactory : IUserRepositoryFactory
{
    // Used to access the current HTTP request (including headers).
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    // Used to manually resolve the transient concrete repository class.
    private readonly IServiceProvider _serviceProvider;
    
    // Stores the resolved database type to avoid reading the header multiple times per request.
    private readonly string _dbType; 

    public UserRepositoryFactory(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;

        // Resolve the DB type once in the constructor. This runs per-request (Scoped lifetime).
        _dbType = _httpContextAccessor.HttpContext
                            ?.Request.Headers["X-Database-Type"]
                            .FirstOrDefault()
                            ?.ToLowerInvariant() ?? "mysql"; // Defaults to MySQL if header is missing
    }

    /// Retrieves the correct IUserRepository based on the stored database type.
    public IUserRepository GetUserRepository()
    {
        return _dbType switch
        {
            // Note: We use GetRequiredService<T>() here to resolve the concrete Transient registrations
            "mongodb" => _serviceProvider.GetRequiredService<MongoDbUserRepository>(),
            "neo4j" => _serviceProvider.GetRequiredService<Neo4jUserRepository>(),
            _ => _serviceProvider.GetRequiredService<MySqlUserRepository>() // Defaults to MySQL
        };
    }
}