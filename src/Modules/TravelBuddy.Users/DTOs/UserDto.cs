namespace TravelBuddy.Users.DTOs
{
    // Public DTO (Data Transfer Object) - This is the safe contract for the API response.
    // It only exposes necessary public data, deliberately hiding sensitive fields like PasswordHash.
    public record UserDto(
        int UserId, 
        string Name, 
        string Email,
        DateOnly Birthdate // Using DateOnly for proper date display without time
    );
}