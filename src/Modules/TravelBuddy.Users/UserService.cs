namespace TravelBuddy.Users
{
    // Public DTO (Data Transfer Object) - This is the safe contract for the API response.
    // It only exposes necessary public data, deliberately hiding sensitive fields like PasswordHash.
    public record UserDto(
        int UserId, 
        string Name, 
        string Email,
        DateOnly Birthdate // Using DateOnly for proper date display without time
    ); 

    // Contract: Defines the public methods available for the Users service.
    public interface IUserService
    {
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