using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users;
// 2. Concrete Repository Implementation: Uses EF Core to perform data access.
public class MySqlUserRepository : IUserRepository
{
    // Dependency Injection: The actual database context (connection) is injected here.
    private readonly UsersDbContext _context;

    public MySqlUserRepository(UsersDbContext context)
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
    public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int userId, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash)) return (false, "Password hash is required.");
        try {
            var result = await _context.Database.ExecuteSqlInterpolatedAsync($@"CALL delete_user({userId}, {passwordHash})");
            return (true, null);
        } catch (Exception ex) {
            return (false, ex.Message ?? "An error occurred while deleting the user.");
        }
    }
    public async Task<(bool Success, string? ErrorMessage)> UpdatePasswordAsync(int userId, string passwordHash)
    {
        try {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) return (false, "User not found.");

            user.PasswordHash = passwordHash;
            await _context.SaveChangesAsync();
            return (true, null);

        } catch (Exception ex) {
            return (false, ex.Message ?? "An error occurred while updating the password.");
        }
    }
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
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
    // ------------------------------- AUDIT TABLES -------------------------------
    public async Task<IEnumerable<UserAudit>> GetUserAuditsAsync()
    {
        return await _context.UserAudits
            .AsNoTracking()
            .ToListAsync();
    }
}