## 🚀 Endpoint Flow Explanation: `GET /api/users`

This process follows the principle of **Separation of Concerns (SoC)**, where each file/layer is responsible for only one specific task.

### 1\. The Presentation Layer: `UsersController.cs`

This file is the **entry point**—it handles the external HTTP request and response contract.

```csharp
// File: src/TravelBuddy.Api/Controllers/UsersController.cs

// 1. RECEIVES THE REQUEST
[ApiController]
[Route("api/[controller]")] // Sets the base route: /api/users
public class UsersController : ControllerBase
{
    private readonly IUserService _userService; // 2. DEPENDENCY: Needs the business layer contract

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet] // 3. HANDLES THE HTTP GET VERB
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        // 4. DELEGATES: Calls the business logic service layer
        var users = await _userService.GetAllUsersAsync(); 

        // 5. FORMATS RESPONSE: Converts the result into the appropriate HTTP response
        return Ok(users); // Returns HTTP 200 OK with UserDto data
    }
}
```

  - **Purpose:** Receives the client request, finds the required service (`IUserService`) from the Dependency Injection container, and calls the appropriate business method (`GetAllUsersAsync`). It then translates the result into an HTTP response (e.g., 200 OK or 204 No Content).
  - **Key Takeaway:** The Controller only deals with **HTTP concerns** (routes, verbs, status codes) and **delegates business logic**.

-----

### 2\. The Application/Business Layer: `UserService.cs`

This file contains the **core business logic** and maps domain entities to public data transfer objects (DTOs).

```csharp
// File: src/Modules/TravelBuddy.Users/UserService.cs

// 1. DTO CONTRACT: Defines the safe data structure for the API response
public record UserDto(...); 

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository; // 2. DEPENDENCY: Needs the data access contract

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        // 3. DELEGATES: Calls the data access layer (Repository)
        var users = await _userRepository.GetAllAsync();
        
        // 4. MAPPING/TRANSFORMATION: Converts private User entities to public User DTOs
        // This step hides sensitive data like PasswordHash from the API response.
        return users.Select(u => new UserDto(...)).ToList();
    }
}
```

  - **Purpose:** Executes the business task "Get All Users." It calls the repository to fetch the data and then maps the internal domain objects (`User`) to the external presentation objects (`UserDto`).
  - **Key Takeaway:** The Service deals with **business logic** and **data transformation**. It is deliberately unaware of HTTP or database specifics.

-----

### 3\. The Data Access Layer (Abstraction): `UsersRepository.cs`

This file is the **contract and implementation** for data retrieval, keeping the service layer decoupled from the database technology.

```csharp
// File: src/Modules/TravelBuddy.Users/UsersRepository.cs

// 1. REPOSITORY CONTRACT: Defines what data operations the service can request
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
}

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context; // 2. DEPENDENCY: Needs the EF Core context

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        // 3. EXECUTES QUERY: Uses the DbContext to query the database
        return await _context.Users
                             .AsNoTracking()
                             .ToListAsync(); // Returns a list of User entities
    }
}
```

  - **Purpose:** Implements the `IUserRepository` contract using Entity Framework Core. It constructs and executes the database query.
  - **Key Takeaway:** The Repository is the boundary between the **business logic** and the **persistence mechanism**.

-----

### 4\. The Persistence Layer: `UsersDbContext.cs`

This file is the **Entity Framework Core connection** and configuration to the database.

```csharp
// File: src/Modules/TravelBuddy.Users/Infrastructure/UsersDbContext.cs

public class UsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!; // 1. Defines the 'Users' collection for querying

    // 2. CONFIGURATION: Maps the C# User entity to the database table/columns
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>();
        userEntity.ToTable("user"); // Maps to the database table named 'user'
        userEntity.Property(u => u.Email).HasIndex(u => u.Email).IsUnique(); 
        // ... (Sets up column names and constraints) ...
    }
}
```

  - **Purpose:** Acts as the unit of work and session for the database. It contains the **`DbSet<User>`** collection that the Repository queries, and it configures how the `User` class maps to the physical database structure.
  - **Key Takeaway:** The DbContext handles **database configuration and session management**.

#### Why the need for constraints?

The reason you have to add constraints in your `UsersDbContext` isn't just for runtime data access; it's because this class serves two critical functions: **runtime bridge** and **schema architect**.

In short: **You add constraints here to define the *schema* of your database, not just to access the existing one.**

Here is the detailed breakdown of the two roles of the `UsersDbContext` and why the constraints are necessary.

---

##### 1. Role A: Defining the Database Schema (Migrations)

The primary reason for the `OnModelCreating` method and the **Fluent API** configuration is to define the exact structure of your database tables, columns, and constraints.

When you use EF Core's **Migration** tools (`dotnet ef migrations add...`), the framework inspects your `UsersDbContext` and translates all the C# configurations into SQL commands.

| Fluent API Line | What it Creates in SQL | Why it's Needed |
| :--- | :--- | :--- |
| `userEntity.ToTable("user")` | **`CREATE TABLE user ...`** | Tells EF Core the name of the physical table in the database. |
| `userEntity.Property(u => u.Id).HasColumnName("user_id")` | **`user_id INT PRIMARY KEY`** | Tells EF Core the exact column name in the database. |
| `userEntity.Property(u => u.Name).IsRequired()` | **`name NVARCHAR(100) NOT NULL`** | Enforces that this column must always have a value in the database. |
| `userEntity.HasIndex(u => u.Email).IsUnique()` | **`CREATE UNIQUE INDEX`** | Ensures that no two rows in the database can have the same email address, enforcing **data integrity**. |

Without these constraints in `OnModelCreating`, EF Core would try to guess the schema, and it would miss critical rules like the unique email index or the specific maximum lengths for columns.

---

###### 2. Role B: Enhancing Runtime Access (Validation)

While the Repository (`UsersRepository.cs`) uses the `UsersDbContext` to fetch data, the `DbContext` can also enforce integrity rules **before** it even hits the database.

**Data Validation**

When your service attempts to save a new `User` entity to the database, the `DbContext` can use the constraints defined in `OnModelCreating` to perform checks:

1.  **Length Check:** If you try to set a `Name` that is longer than 100 characters, the `DbContext` will know this is an invalid state based on the `HasMaxLength(100)` configuration.
2.  **Uniqueness Check:** When you try to insert an email that already exists, the database throws an exception, and EF Core translates that specific database error into a more manageable `DbUpdateException` for your C# code to handle.

By defining all these rules in the `DbContext`, you establish a single source of truth for the **entire data contract**, shared between your C# application and the physical database. This makes your application more robust and simplifies your repository code.

-----

### 5\. The Domain Layer: `User.cs`

This file represents the **core data structure** and **behavior** of a user.

```csharp
// File: src/Modules/TravelBuddy.Users/User.cs

public class User : BaseEntity<int> 
{
    public string Name { get; private set; } = null!; // Read-only properties
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    private User() { } // Private constructor enforces object creation via a factory
    
    // public static User CreateNew(...) // Factory method enforces invariants
    // public void UpdateProfile(...) // Business behavior method
}
```

  - **Purpose:** The canonical model for a user in the system. It enforces data integrity by using **private setters** (making it immutable outside the class) and factory methods.
  - **Key Takeaway:** The Entity deals with **data and behavior** that belong to the domain itself. It is the object passed between the Repository and the Service.


## How to run the backend

1. Write `dotnet run`
2. Go to either `https://localhost:7164/swagger/index.html` or `https://localhost:7164/api/users`

### If on a new device

1. Write `cd src/TravelBuddy.Api`
2. Get the dependencies: `dotnet restore`
3. Continue with the instructions above aka `dotnet run`