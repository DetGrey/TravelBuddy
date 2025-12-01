using Neo4j.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users;

public class Neo4jUserRepository : IUserRepository
{
    private readonly IDriver _driver;

    public Neo4jUserRepository(IDriver driver)
    {
        _driver = driver;
    }

    // ----------------- Helpers -----------------

    private static User MapRecordToUser(IRecord record)
    {
        DateOnly birthdate = default;

        if (record.Keys.Contains("birthdate") && record["birthdate"] is LocalDate ld)
        {
            birthdate = new DateOnly(ld.Year, ld.Month, ld.Day);
        }

        return new User
        {
            UserId = record["userId"].As<int>(),
            Name = record["name"].As<string>(),
            Email = record["email"].As<string>(),
            PasswordHash = record["passwordHash"].As<string>(),
            Birthdate = birthdate,   // default(DateOnly) if not present
            IsDeleted = record["isDeleted"].As<bool>(),
            Role = record["role"].As<string>()
        };
    }

    private async Task<int> GetNextUserIdAsync()
    {
        var result = await _driver
            .ExecutableQuery(@"
                MATCH (u:User)
                RETURN coalesce(max(u.userId), 0) AS maxId
            ")
            .ExecuteAsync();

        var record = result.Result.SingleOrDefault();
        var maxId = record == null ? 0 : record["maxId"].As<int>();
        return maxId + 1;
    }

    // ----------------- IRepository methods -----------------

    public async Task<User?> GetByEmailAsync(string email)
    {
        var result = await _driver
            .ExecutableQuery(@"
                MATCH (u:User { email: $email })
                WHERE coalesce(u.isDeleted, false) = false
                RETURN u.userId      AS userId,
                       u.name        AS name,
                       u.email       AS email,
                       u.passwordHash AS passwordHash,
                       u.birthdate   AS birthdate,
                       u.isDeleted   AS isDeleted,
                       u.role        AS role
                LIMIT 1
            ")
            .WithParameters(new { email })
            .ExecuteAsync();

        var record = result.Result.SingleOrDefault();
        return record == null ? null : MapRecordToUser(record);
    }

    public async Task AddAsync(User user)
    {
        // Auto-generate userId like in Mongo if not set
        if (user.UserId == 0)
        {
            user.UserId = await GetNextUserIdAsync();
        }

        await _driver
            .ExecutableQuery(@"
                CREATE (u:User {
                    userId: $userId,
                    name: $name,
                    email: $email,
                    passwordHash: $passwordHash,
                    birthdate: date($birthdate),
                    isDeleted: $isDeleted,
                    role: $role
                })
            ")
            .WithParameters(new
            {
                userId = user.UserId,
                name = user.Name,
                email = user.Email,
                passwordHash = user.PasswordHash,
                birthdate = user.Birthdate.ToString("yyyy-MM-dd"),
                isDeleted = user.IsDeleted,
                role = user.Role
            })
            .ExecuteAsync();
    }

    public async Task<bool> DeleteAsync(int userId, string passwordHash)
    {
        var result = await _driver
            .ExecutableQuery(@"
                MATCH (u:User { userId: $userId })
                SET u.isDeleted   = true,
                    u.passwordHash = $passwordHash
                RETURN count(u) AS updatedCount
            ")
            .WithParameters(new { userId, passwordHash })
            .ExecuteAsync();

        var record = result.Result.SingleOrDefault();
        if (record == null) return false;

        var updatedCount = record["updatedCount"].As<long>();
        return updatedCount == 1;
    }

    public async Task UpdatePasswordAsync(int userId, string passwordHash)
    {
        await _driver
            .ExecutableQuery(@"
                MATCH (u:User { userId: $userId })
                WHERE coalesce(u.isDeleted, false) = false
                SET u.passwordHash = $passwordHash
            ")
            .WithParameters(new { userId, passwordHash })
            .ExecuteAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var result = await _driver
            .ExecutableQuery(@"
                MATCH (u:User { userId: $userId })
                WHERE coalesce(u.isDeleted, false) = false
                RETURN u.userId      AS userId,
                       u.name        AS name,
                       u.email       AS email,
                       u.passwordHash AS passwordHash,
                       u.birthdate   AS birthdate,
                       u.isDeleted   AS isDeleted,
                       u.role        AS role
                LIMIT 1
            ")
            .WithParameters(new { userId })
            .ExecuteAsync();

        var record = result.Result.SingleOrDefault();
        return record == null ? null : MapRecordToUser(record);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var result = await _driver
            .ExecutableQuery(@"
                MATCH (u:User)
                WHERE coalesce(u.isDeleted, false) = false
                RETURN u.userId      AS userId,
                       u.name        AS name,
                       u.email       AS email,
                       u.passwordHash AS passwordHash,
                       u.birthdate   AS birthdate,
                       u.isDeleted   AS isDeleted,
                       u.role        AS role
                ORDER BY u.userId
            ")
            .ExecuteAsync();

        return result.Result.Select(MapRecordToUser).ToList();
    }
}

/*
public async Task<IEnumerable<User>> GetAllAsync()
    {
        // TODO Placeholder: Return an empty list of Users
        /* // Retrieve all Person nodes who know other persons
        var result = await _driver.ExecutableQuery(@"
            MATCH (p:Person)-[:KNOWS]->(:Person)
            RETURN p.name AS name
            ")
            .WithConfig(new QueryConfig(database: "neo4j"))
            .ExecuteAsync();

        // Loop through results and print people's name
        foreach (var record in result.Result) {
            Console.WriteLine(record.Get<string>("name"));
        }

        // Summary information
        var summary = result.Summary;
        Console.WriteLine($"The query `{summary.Query.Text}` returned {result.Result.Count()} results in {summary.ResultAvailableAfter.Milliseconds} ms.");
        
        return await Task.FromResult<IEnumerable<User>>(new List<User>());
    }
*/