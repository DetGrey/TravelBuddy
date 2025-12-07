# Integration Testing
## Setting up Integration .NET Project
```bash
dotnet new xunit -n TravelBuddy.IntegrationTests
cd TravelBuddy.IntegrationTests

# The engine that spins up your API in memory
dotnet add package Microsoft.AspNetCore.Mvc.Testing

# Helps write assertions like "result.Should().Be(true)"
dotnet add package FluentAssertions

# Create a docker container to run the tests
dotnet add package Testcontainers.MySql

dotnet add reference ../../src/TravelBuddy.Api/TravelBuddy.Api.csproj
dotnet add reference ../../src/Modules/TravelBuddy.Users/TravelBuddy.Users.csproj
dotnet add reference ../../src/Modules/TravelBuddy.Trips/TravelBuddy.Trips.csproj
dotnet add reference ../../src/Modules/TravelBuddy.Messaging/TravelBuddy.Messaging.csproj
dotnet add reference ../../src/Shared/TravelBuddy.SharedKernel/TravelBuddy.SharedKernel.csproj
```

### Setting Up WebApplicationFactory

The `TravelBuddyApiFactory` class uses `WebApplicationFactory<TProgram>` and `Testcontainers.MySql` to:
1. Spin up the API in memory
2. Start a Docker MySQL container for each test run
3. Load SQL schema and seed data from `DbScripts/` folder
4. Isolate tests so they don't affect your production database

The factory configures all DbContexts:
- `UsersDbContext`
- `TripsDbContext`  
- `MessagingDbContext`
- `SharedKernelDbContext`

Each test gets a fresh MySQL container, ensuring test isolation.

## How to run the tests

Run following in the terminal inside this folder
```bash
dotnet test
```

To filter the tests (e.g. only run register tests)
```bash
dotnet test --filter "register"
```