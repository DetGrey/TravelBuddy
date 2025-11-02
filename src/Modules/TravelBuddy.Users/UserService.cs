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

        Task<bool> ChangePasswordAsync(PasswordChangeRequestDto request, string email, int userId);

        // Gets a list of all users from the database.
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }

    // Implementation: Contains the actual business logic.
    public class UserService : IUserService
    {
        // Dependency Injection: This service requires the IUserRepository to fetch data.
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsDeleted) return null;

            bool isValid = PasswordHasher.VerifyPassword(password, user.PasswordHash);
            return isValid ? user : null;
        }

        public async Task<User?> RegisterAsync(RegisterRequestDto request)
        {
            var existing = await _userRepository.GetByEmailAsync(request.Email);
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

            await _userRepository.AddAsync(newUser);

            return newUser;
        }

        public async Task<bool> ChangePasswordAsync(PasswordChangeRequestDto request, string email, int userId)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsDeleted) return false;

            // Note: To actually change password from generated users, comment out the two lines below
            bool isValid = PasswordHasher.VerifyPassword(request.OldPassword, user.PasswordHash);
            if (!isValid) return false;

            var hashedPassword = PasswordHasher.HashPassword(request.NewPassword);

            await _userRepository.UpdatePasswordAsync(userId, hashedPassword);

            return true;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            // 1. Delegate to the Data Layer (Repository) to fetch the raw User entities.
            var users = await _userRepository.GetAllAsync();

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
    }
}