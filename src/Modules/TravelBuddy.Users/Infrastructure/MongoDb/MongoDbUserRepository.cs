using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users;
public class MongoDbUserRepository : IUserRepository
{
    // Declare the IMongoCollection<T> field. This is your "data access object."
    private readonly IMongoCollection<User> _usersCollection; 

    // Inject IMongoClient instead of UsersDbContext.
    public MongoDbUserRepository(IMongoClient client)
    {
        // Use the client to get the database and then the specific collection.
        var database = client.GetDatabase("travel_buddy_mongo");
        _usersCollection = database.GetCollection<User>("users");
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
        // TODO Placeholder: Return false indicating the deletion didn't happen
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
        return await Task.FromResult<IEnumerable<User>>(new List<User>());
    }
}