namespace TravelBuddy.Users.DTOs
{
    public record AuthResponseDto(
        UserDto User,
        string Token
    );
}