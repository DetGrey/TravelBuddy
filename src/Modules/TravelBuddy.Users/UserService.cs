using TravelBuddy.Users.DTOs;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure.Security;

namespace TravelBuddy.Users
{
    // Contract: Defines the public methods available for the Users service.
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User?> RegisterAsync(RegisterRequestDto request);
        Task<bool> DeleteUser(int userId);
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<bool> ChangePasswordAsync(PasswordChangeRequestDto request, string email, int userId);

        // Gets a list of all users from the database.
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<UserAuditDto>> GetUserAuditsAsync();
    }

    // Implementation: Contains the actual business logic.
    public class UserService : IUserService
    {
        private readonly IUserRepositoryFactory _userRepositoryFactory;

        public UserService(IUserRepositoryFactory userRepositoryFactory)
        {
            _userRepositoryFactory = userRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private IUserRepository GetRepo() => _userRepositoryFactory.GetUserRepository();

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var userRepository = GetRepo();

            var user = await userRepository.GetByEmailAsync(email);
            if (user == null || user.IsDeleted) return null;

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<User?> RegisterAsync(RegisterRequestDto request)
        {
            var userRepository = GetRepo();

            var existing = await userRepository.GetByEmailAsync(request.Email);
            if (existing != null) return null; // Email already in use

            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Birthdate = request.Birthdate,
                Role = "user"
            };

            await userRepository.AddAsync(newUser);

            return newUser;
        }
        
        public async Task<bool> DeleteUser(int userId)
        {
            var userRepository = GetRepo();

            var placeholder = $"deleted_{userId}_{DateTime.UtcNow:yyyyMMddHHmmss}";
            var hashedPassword = PasswordHasher.HashPassword(placeholder);
            return await userRepository.DeleteAsync(userId, hashedPassword);
        }

        public async Task<bool> ChangePasswordAsync(PasswordChangeRequestDto request, string email, int userId)
        {
            var userRepository = GetRepo();

            var user = await userRepository.GetByEmailAsync(email);
            if (user == null || user.IsDeleted) return false;

            // Note: To actually change password from generated users, comment out the two lines below
            bool isValid = PasswordHasher.VerifyPassword(request.OldPassword, user.PasswordHash);
            if (!isValid) return false;

            var hashedPassword = PasswordHasher.HashPassword(request.NewPassword);
            await userRepository.UpdatePasswordAsync(userId, hashedPassword);

            return true;
        }
        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var userRepository = GetRepo();
            var user = await userRepository.GetUserByIdAsync(userId);
            if (user == null) return null;
            return new UserDto(
                user.UserId,
                user.Name,
                user.Email,
                user.Birthdate
            );
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var userRepository = GetRepo();

            // 1. Delegate to the Data Layer (Repository) to fetch the raw User entities.
            var users = await userRepository.GetAllAsync();

            // 2. Map Entities to DTOs (Data Transformation).
            // This converts the internal 'User' object (which has PasswordHash) 
            // into the safe 'UserDto' (which does not).
            return users.Select(u => new UserDto(
                u.UserId,
                u.Name,
                u.Email,
                u.Birthdate
            )).ToList();
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<UserAuditDto>> GetUserAuditsAsync()
        {
            var userRepository = GetRepo();
            var results = await userRepository.GetUserAuditsAsync();
            return results.Select(ua => new UserAuditDto(
                ua.AuditId,
                ua.UserId,
                ua.Action,
                ua.FieldChanged,
                ua.OldValue,
                ua.NewValue,
                ua.ChangedBy,
                ua.Timestamp
            )).ToList();
        }
    }
}