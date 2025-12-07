using DotNet.Testcontainers.Builders;
using Testcontainers.MySql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Messaging.Infrastructure;
using TravelBuddy.SharedKernel.Infrastructure;

// Add IAsyncLifetime to handle Docker startup/shutdown
public class TravelBuddyApiFactory<TProgram> 
    : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    private static readonly string BaseDir = AppContext.BaseDirectory;
    // Define paths to SQL files
    private static readonly string TablesSqlPath = Path.Combine(BaseDir, "DbScripts", "1_create_db_tables.sql");
    private static readonly string ObjectsSqlPath = Path.Combine(BaseDir, "DbScripts", "2_stored_objects.sql");

    private readonly MySqlContainer _mySqlContainer = new MySqlBuilder()
        .WithImage("mysql:8.0")
        .WithReuse(true) // Enable container reuse for faster tests
        .WithDatabase("travel_buddy")
        .WithUsername("testuser")
        .WithPassword("testpass")
        // Map them to run in the correct order (01 first, then 02)
        .WithResourceMapping(File.ReadAllBytes(TablesSqlPath), "/docker-entrypoint-initdb.d/01_tables.sql")
        .WithResourceMapping(File.ReadAllBytes(ObjectsSqlPath), "/docker-entrypoint-initdb.d/02_objects.sql")
        // 1. Put the database storage in RAM. 
        // This bypasses the slow WSL2 hard drive I/O completely.
        .WithTmpfsMount("/var/lib/mysql")
        // 2. Disable heavy data safety features. 
        // We don't care about data recovery if the test crashes, we want speed.
        .WithCommand(
            "--innodb_flush_log_at_trx_commit=2",  // Don't flush to disk immediately
            "--skip-log-bin"                       // Disable binary logging
        )
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // -------------------------------------------------------------
            // A. Remove Existing DbContext Options (Production/Dev SQL)
            // -------------------------------------------------------------
            var dbContextDescriptors = services.Where(
                d => d.ServiceType.IsGenericType &&
                     d.ServiceType.GetGenericTypeDefinition() == typeof(DbContextOptions<>)
            ).ToList();

            foreach (var descriptor in dbContextDescriptors)
            {
                services.Remove(descriptor);
            }

            // -------------------------------------------------------------
            // B. Add DbContexts pointing to the Docker Container
            // -------------------------------------------------------------
            var connectionString = _mySqlContainer.GetConnectionString();

            // Note: We use the SAME connection string for all contexts 
            // because your Docker container is one single database server.
            
            services.AddDbContext<UsersDbContext>(options => 
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<TripsDbContext>(options => 
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<MessagingDbContext>(options => 
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddDbContext<SharedKernelDbContext>(options => 
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        });
    }

    // Start container before any tests run
    public async Task InitializeAsync() => await _mySqlContainer.StartAsync();

    // Stop container after all tests are done
    public new async Task DisposeAsync() => await _mySqlContainer.StopAsync();
}