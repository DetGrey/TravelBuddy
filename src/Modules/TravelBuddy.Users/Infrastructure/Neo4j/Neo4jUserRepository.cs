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
    public async Task<User?> GetByEmailAsync(string email)
    {
        // TODO Placeholder: Return null for the User object
        return await Task.FromResult<User?>(null);
    }

    public async Task AddAsync(User user)
    {
        // TODO Placeholder: Return a completed Task with no result
        await Task.CompletedTask;
    }

    public async Task<bool> DeleteAsync(int userId, string passwordHash)
    {
        // TODO Placeholder: Return false indicating the deletion didn't happen (or true if needed)
        return await Task.FromResult(false);
    }

    public async Task UpdatePasswordAsync(int userId, string passwordHash)
    {
        // TODO Placeholder: Return a completed Task with no result
        await Task.CompletedTask;
    }
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        // TODO Placeholder: Return user
        return await Task.FromResult<User?>(null);
    }
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
        */
        return await Task.FromResult<IEnumerable<User>>(new List<User>());
    }
}