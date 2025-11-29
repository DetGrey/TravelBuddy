using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users;

// 1. Repository Interface (The Contract): Defines what operations are supported.
public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task<bool> DeleteAsync(int userId, string passwordHash);
    Task UpdatePasswordAsync(int userId, string passwordHash);
    Task<User?> GetUserByIdAsync(int userId);
    Task<IEnumerable<User>> GetAllAsync();
}