## Hibernate, Entity Framework and Sequelize explanation

**Hibernate**, **Entity Framework**, and **Sequelize** are all distinct **Object-Relational Mapping (ORM) tools**.

You choose which one to use depending on the programming language or framework you are working with:

*   **Entity Framework** is the ORM for **.NET** (C#).
*   **Hibernate** is the ORM for **Java** (specifically, an implementation of JPA).
*   **Sequelize** is an ORM used for **JavaScript** environments, such as Node.js.

They are alternatives used to abstract the database, allowing you to query via Object-Relational Mapping Classes rather than raw SQL.

---

# TravelBuddy EF Core Architecture Overview

This project uses a **modular monolith** architecture with **separate DbContexts per module**, following a clean separation of concerns. Each module owns its own data model and persistence logic, while shared entities and cross-cutting concerns are handled via foreign keys or shared contexts.

## Module Structure
```
src/
├── Modules/
│   ├── TravelBuddy.Users/
│   │   ├── Models/               # User entity
│   │   ├── Infrastructure/
│   │   │   └── UsersDbContext.cs
│   │   └── UserService.cs, IUserRepository.cs, etc.
│   │
│   ├── TravelBuddy.Trips/
│   │   ├── Models/               # Trip, TripAudit, Buddy, etc.
│   │   ├── Infrastructure/
│   │   │   └── TripsDbContext.cs
│   │   └── TripService.cs, etc.
│   │
│   ├── TravelBuddy.Messaging/
│   │   ├── Models/               # Message, Conversation, etc.
│   │   ├── Infrastructure/
│   │   │   └── MessagingDbContext.cs
│   │   └── MessagingService.cs, etc.
│
├── Shared/
│   └── TravelBuddy.SharedKernel/
│       ├── Models/               # SystemEventLog, etc.
│       ├── Infrastructure/
│       │   └── SharedKernelDbContext.cs
│
├── TravelBuddy.Api/
│   ├── Controllers/              # API endpoints (e.g., UsersController)
│   ├── Program.cs                # Application startup and DI configuration
```

## DbContext Responsibilities

| Context                  | Module or Shared     | Entities Owned                          | Notes |
|--------------------------|----------------------|------------------------------------------|-------|
| `UsersDbContext`         | Users                | `User`                                   | No navigation to other modules |
| `TripsDbContext`         | Trips                | `Trip`, `TripAudit`, `Buddy`, `BuddyAudit` | Uses `UserId` as FK only — no navigation |
| `MessagingDbContext`     | Messaging            | `Message`, `Conversation`, `ConversationAudit` | References `UserId`, `TripDestinationId` via FK |
| `SharedKernelDbContext`  | SharedKernel         | `SystemEventLog`                         | Used for cross-cutting logging |

## Cross-Module Relationships

- All cross-module relationships are handled via **foreign keys only** (e.g., `UserId`, `TripDestinationId`)
- **No navigation properties** are defined across module boundaries to avoid circular dependencies
- Queries are performed using `.Where(x => x.UserId == ...)` rather than `user.Trips`

## 🛠️ Scaffolding Strategy

Each module scaffolds only the tables it owns, plus any required foreign key references. After scaffolding:
- Unused entities are deleted
- Shared entities (like `User`) are referenced via project dependencies and `using` directives
- Navigation properties are simplified to `.WithMany()` when inverse navigation is not needed

## Example Query (No Navigation)

```csharp
// In Trips module
var trips = await _context.Trips
    .Where(t => t.UserId == userId)
    .ToListAsync();
```


## Benefits of This Setup

- Clear ownership of entities per module
- No circular project references
- Easy to scale or split into microservices later
- Clean separation of domain logic and persistence

---

# Make Database-first ORM

## 1. For each Module, run these inside the correct Module folder (e.g. TravelBuddy.Users): 

> Note: To run `dotnet ef` you have to install it: `dotnet tool install --global dotnet-ef`

> Note that each module/project you add DbContext to needs to have following in the `.csproj`:
```
<ItemGroup>
    <PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.10" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.10" />
    <PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
</ItemGroup>
```

> Note: adding `--force` makes it possible to overwrite old files

**USERS**
```bash
dotnet ef dbcontext scaffold "Your_Connection_String" Pomelo.EntityFrameworkCore.MySql \
  --context UsersDbContext \
  --output-dir Models \
  --context-dir Infrastructure \
  --namespace TravelBuddy.Users.Models \
  --context-namespace TravelBuddy.Users.Infrastructure \
  --table user \
  --no-onconfiguring \
  --force
```

**MESSAGES**
```bash
dotnet ef dbcontext scaffold "Your_Connection_String" Pomelo.EntityFrameworkCore.MySql \
  --context MessagingDbContext \
  --output-dir Models \
  --context-dir Infrastructure \
  --namespace TravelBuddy.Messaging.Models \
  --context-namespace TravelBuddy.Messaging.Infrastructure \
  --table message \
  --table conversation_participant \
  --table conversation \
  --table conversation_audit \
  --table user \
  --table trip_destination \
  --table destination \
  --table trip \
  --no-onconfiguring \
  --force
```

**TRIPS**
```bash
dotnet ef dbcontext scaffold "Your_Connection_String" Pomelo.EntityFrameworkCore.MySql \
  --context TripsDbContext \
  --output-dir Models \
  --context-dir Infrastructure \
  --namespace TravelBuddy.Trips.Models \
  --context-namespace TravelBuddy.Trips.Infrastructure \
  --table user \
  --table trip_destination \
  --table destination \
  --table trip \
  --table buddy \
  --table buddy_audit \
  --table trip_audit \
  --no-onconfiguring \
  --force
```

**SHARED**

> Note: This should be done inside "src/Shared/TravelBuddy.SharedKernel"

```bash
dotnet ef dbcontext scaffold "Your_Connection_String" Pomelo.EntityFrameworkCore.MySql \
  --context SharedKernelDbContext \
  --output-dir Models \
  --context-dir Infrastructure \
  --namespace TravelBuddy.SharedKernel.Models \
  --context-namespace TravelBuddy.SharedKernel.Infrastructure \
  --table system_event_log \
  --no-onconfiguring
```

## 2. Delete the Models that are in other Modules

This is done manually where e.g. User.cs is removed from Trips and Messaging.

## 3. Make references to the other modules to access the Models needed

> Note: This should be done from repo root

Adding Users to Messaging
```bash
dotnet add src/Modules/TravelBuddy.Messaging/TravelBuddy.Messaging.csproj reference src/Modules/TravelBuddy.Users/TravelBuddy.Users.csproj
```

Adding Users to Trips
```bash
dotnet add src/Modules/TravelBuddy.Trips/TravelBuddy.Trips.csproj reference src/Modules/TravelBuddy.Users/TravelBuddy.Users.csproj
```

Adding Trips to Messaging
```bash
dotnet add src/Modules/TravelBuddy.Messaging/TravelBuddy.Messaging.csproj reference src/Modules/TravelBuddy.Trips/TravelBuddy.Trips.csproj
```

In TripsDbContext add:
```csharp
using TravelBuddy.Users.Models;
```
Also write `.WithMany()` with empty brackets instead of non-empty in the places that refers to the user table

In MessagingDbContext add:
```csharp
using TravelBuddy.Trips.Models;
using TravelBuddy.Users.Models;
```
Also write `.WithMany()` with empty brackets instead of non-empty in the places that refers to the user or trip table

> In many Models, there need to be added references too if they have a foreign key to tables related to Trips or Users. This is done like how it was added to the TripsDbContext and MessagingDbContext above.

## 4. Edit Program.cs

Add namespace references:
```csharp
using TravelBuddy.Users; 
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Trips; 
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Messaging; 
using TravelBuddy.Messaging.Infrastructure;
```

Add this after getting the ConnectionString from env variables
```csharp
// Register the Users DbContext
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(UsersDbContext).Assembly.FullName)
    )
);

// Register the Trips DbContext
builder.Services.AddDbContext<TripsDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(TripsDbContext).Assembly.FullName)
    )
);

// Register the Messaging DbContext
builder.Services.AddDbContext<MessagingDbContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString),
        b => b.MigrationsAssembly(typeof(MessagingDbContext).Assembly.FullName)
    )
);
```