
## Writing convention for our project

| Layer               | Convention     | Example                         |
|---------------------|----------------|---------------------------------|
| API endpoint path   | `kebab-case`   | `/api/trip-destinations/search` |
| Query parameters    | `camelCase`    | `?reqStart=2025-03-01`          |
| C# properties       | `PascalCase`   | `TripDestinationId`             |
| SQL aliases         | `PascalCase`   | `AS TripDestinationId`          |
| DB schema           | `snake_case`   | `trip_destination_id`           |


## DTO response vs request types

| **DTO type**         | **Purpose**              | **Best Practice**             | 
|----------------------|--------------------------|-------------------------------|
| `UserDto`            | Response DTO (read-only) | Use `record` for immutability | 
| `RegisterRequestDto` | Request DTO (writeable)  | Use `class` with validation   | 


## How to add new endpoints (shown through example)

This example is for seeing all trips from a user AKA endpoint "api/users/{id}/trip-destinations"

### 1. Make or choose existing model

Here I had to make a new model since the data is different from just a TripDestination

```csharp
namespace TravelBuddy.Trips.Models
{
    public class UserTripSummary
    {
        public int TripId { get; set; }
        public int TripDestinationId { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public string TripDescription { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "owner" or "buddy"
    }
}
```

#### 1.1 Add new model to context (ignore if reusing a model)

Add following where the other similar ones are at the top:
```csharp
public DbSet<UserTripSummary> UserTripSummaries { get; set; }
```

Add following at the bottom under the last of the same kind:
```csharp
modelBuilder.Entity<UserTripSummary>().HasNoKey();
```

### 2. Add task to Repository

First add task to interface:
```csharp
Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId);
```

Then add task to class:
```csharp
public async Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId)
{
    return await _context.UserTripSummaries
        .FromSqlInterpolated($@"
            CALL get_user_trips({userId})")
        .AsNoTracking()
        .ToListAsync();
}
```

### 3. Add task to Service

Make DTO if you created a new class else skip:
```csharp
public record UserTripSummaryDto(
        int TripId,
        int TripDestinationId,
        string DestinationName,
        string TripDescription,
        string Role
    );
```

Add task to interface:
```csharp
Task<IEnumerable<UserTripSummaryDto>> GetUserTripsAsync(int userId);
```

Add task to class:
```csharp
public async Task<IEnumerable<UserTripSummaryDto>> GetUserTripsAsync(int userId)
{
    var results = await _repository.GetUserTripsAsync(userId);

    return results.Select(r => new UserTripSummaryDto(
        r.TripId,
        r.TripDestinationId,
        r.DestinationName,
        r.TripDescription,
        r.Role
    )).ToList();
}
```

### 4. Add endpoint to Controller

```csharp
// GET /api/users/{id}/trip-destinations
// The "{id}" parameter in the HttpGet attribute maps the URL segment to the 'id' parameter below.
[HttpGet("{id}/trip-destinations")]
[ProducesResponseType(typeof(UserDto), 200)] // Success response type
[ProducesResponseType(404)]                   // Not Found response
public async Task<ActionResult<UserDto>> GetUserTrips([FromRoute] int id)
{
    var tripDestinations = await _tripDestinationService.GetUserTripsAsync(id);
    if (!tripDestinations.Any()) return NoContent();
    return Ok(tripDestinations);
}
```

### Extra (if using a new service in the controller)

Add following to Controller

```csharp
using TravelBuddy.Trips;
```

```csharp
private readonly ITripDestinationService _tripDestinationService;
```

Update this one to include new service
```csharp
public UsersController(
    IUserService userService,
    ITripDestinationService tripDestinationService
)
{
    _userService = userService;
    _tripDestinationService = tripDestinationService;
}
```