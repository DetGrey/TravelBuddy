using Microsoft.Extensions.DependencyInjection;
using Xunit;
using TravelBuddy.Users.Infrastructure;

// This tag tells xUnit to use the Shared Docker Container
[Collection("Integration Tests")]
public abstract class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly HttpClient _client;
    protected readonly TravelBuddyApiFactory<Program> _factory;

    // Constructor receives the shared Factory
    protected BaseIntegrationTest(TravelBuddyApiFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // This runs AUTOMATICALLY before every test in any class that inherits this
    public async Task InitializeAsync()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
            
            // Clean up the DB to ensure Test Isolation
            db.Users.RemoveRange(db.Users);
            await db.SaveChangesAsync();
        }
    }

    public Task DisposeAsync() => Task.CompletedTask;
}