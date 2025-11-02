using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users
{
    // 1. Repository Interface (The Contract): Defines what operations are supported.
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task UpdatePasswordAsync(int userId, string passwordHash);

        // Method to get all User entities from the persistence store.
        Task<IEnumerable<User>> GetAllAsync();
    }

    // 2. Concrete Repository Implementation: Uses EF Core to perform data access.
    public class UserRepository : IUserRepository
    {
        // Dependency Injection: The actual database context (connection) is injected here.
        private readonly UsersDbContext _context;

        public UserRepository(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdatePasswordAsync(int userId, string passwordHash)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.PasswordHash = passwordHash;
                await _context.SaveChangesAsync();
            }

        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Query the 'Users' table using the DbContext.
            return await _context.Users
                                 // AsNoTracking() is an optimization for read-only operations.
                                 // It tells EF Core not to track changes, making the query faster.
                                 .AsNoTracking()
                                 .ToListAsync(); // Execute the query and return a list of User entities.'
        }
    }
}