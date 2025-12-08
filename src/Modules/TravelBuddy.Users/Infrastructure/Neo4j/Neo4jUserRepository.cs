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

    public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int userId, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash)) return (false, "Password hash is required.");

        try {
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
            if (record == null) return (false, "User not found.");

            var updatedCount = record["updatedCount"].As<long>();
            return updatedCount == 1 ? (true, null) : (false, "User not found.");
        } catch (Exception ex) {
            return (false, ex.Message ?? "An error occurred while deleting the user.");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdatePasswordAsync(int userId, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash)) return (false, "Password hash is required.");
        
        try {
            await _driver
                .ExecutableQuery(@"
                    MATCH (u:User { userId: $userId })
                    WHERE coalesce(u.isDeleted, false) = false
                    SET u.passwordHash = $passwordHash
                ")
                .WithParameters(new { userId, passwordHash })
                .ExecuteAsync();
            return (true, null);
        } catch (Exception ex) {
            return (false, ex.Message ?? "An error occurred while updating the password.");
        }
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

    public async Task<(bool Success, string? ErrorMessage)> AdminDeleteAsync(int userId, int changedBy)
    {
        await using var session = _driver.AsyncSession();
        
        try
        {
            return await session.ExecuteWriteAsync(async tx =>
            {
                // Get user name for audit
                var checkCypher = @"
                    MATCH (u:User {userId: $userId})
                    RETURN u.name as UserName
                ";
                var checkCursor = await tx.RunAsync(checkCypher, new { userId });
                var checkRecords = await checkCursor.ToListAsync();
                
                if (!checkRecords.Any())
                    return (false, "No user found with the given user_id");

                var userName = checkRecords.First()["UserName"].As<string?>();

                // Get next audit ID
                var auditIdCypher = @"
                    MATCH (a:UserAudit)
                    RETURN coalesce(max(a.auditId), 0) + 1 as NextId
                ";
                var auditIdCursor = await tx.RunAsync(auditIdCypher);
                var auditIdRecord = await auditIdCursor.SingleAsync();
                var nextAuditId = auditIdRecord["NextId"].As<int>();

                // Create audit node (independent, not linked to user since user will be deleted)
                var auditCypher = @"
                    CREATE (a:UserAudit {
                        auditId: $auditId,
                        userId: $userId,
                        action: 'deleted',
                        fieldChanged: 'user',
                        oldValue: $userName,
                        newValue: 'PERMANENTLY DELETED',
                        timestamp: datetime()
                    })
                    WITH a
                    MATCH (cb:User {userId: $changedBy})
                    CREATE (cb)-[:CHANGED]->(a)
                ";
                await tx.RunAsync(auditCypher, new { auditId = nextAuditId, userId, userName, changedBy });

                // PERMANENTLY delete the user and all relationships
                var deleteCypher = @"
                    MATCH (u:User {userId: $userId})
                    OPTIONAL MATCH (u)-[r]-()
                    DELETE r, u
                ";
                await tx.RunAsync(deleteCypher, new { userId });

                return (true, (string?)null);
            });
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    // ------------------------------- AUDIT TABLES -------------------------------
    public async Task<IEnumerable<UserAudit>> GetUserAuditsAsync()
    {
        await using var session = _driver.AsyncSession();
        const string cypher = @"
            MATCH (u:User)-[:HAS_AUDIT]->(a:UserAudit)
            OPTIONAL MATCH (cb:User)-[:CHANGED]->(a)
            RETURN a.auditId as AuditId, u.userId as UserId, a.action as Action,
                   a.fieldChanged as FieldChanged, a.oldValue as OldValue,
                   a.newValue as NewValue, cb.userId as ChangedBy,
                   a.timestamp as Timestamp
            ORDER BY a.auditId DESC
        ";
        
        var cursor = await session.RunAsync(cypher);
        var records = await cursor.ToListAsync();
        
        return records.Select(r => new UserAudit
        {
            AuditId = r["AuditId"].As<int?>() ?? 0,
            UserId = r["UserId"].As<int?>() ?? 0,
            Action = r["Action"].As<string?>() ?? string.Empty,
            FieldChanged = r["FieldChanged"].As<string?>(),
            OldValue = r["OldValue"].As<string?>(),
            NewValue = r["NewValue"].As<string?>(),
            ChangedBy = r["ChangedBy"].As<int?>(),
            Timestamp = ParseDateTime(r["Timestamp"])
        }).ToList();
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateUserRoleAsync(int userId, string newRole, int changedBy)
    {
        try {
            var result = await _driver.ExecutableQuery(@"
                MATCH (u:User { userId: $userId })
                WITH u, u.role AS oldRole
                SET u.role = $newRole
                WITH u, oldRole
                OPTIONAL MATCH (maxAudit:UserAudit)
                WITH u, oldRole, coalesce(max(maxAudit.AuditId), 0) AS maxId
                CREATE (audit:UserAudit {
                    AuditId: maxId + 1,
                    UserId: $userId,
                    Action: 'updated',
                    FieldChanged: 'role',
                    OldValue: oldRole,
                    NewValue: $newRole,
                    ChangedBy: $changedBy,
                    Timestamp: datetime()
                })
                RETURN count(u) AS updatedCount
            ")
            .WithParameters(new { userId, newRole, changedBy })
            .ExecuteAsync();

            var records = result.Result;
            if (!records.Any()) return (false, "User not found.");

            var updatedCount = records[0]["updatedCount"].As<long>();
            return updatedCount == 1 ? (true, null) : (false, "User not found.");
        }
        catch (Exception ex) {
            return (false, ex.Message ?? "An error occurred while updating the user role.");
        }
    }
    
    private static DateTime ParseDateTime(object? value)
    {
        if (value == null)
            return DateTime.MinValue;

        if (value is DateTime dt)
            return dt;

        if (value is Neo4j.Driver.LocalDateTime ldt)
            return ldt.ToDateTime();

        if (value is Neo4j.Driver.ZonedDateTime zdt)
            return zdt.UtcDateTime;

        if (value is string s && DateTime.TryParse(s, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
            return parsed;

        return DateTime.MinValue;
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