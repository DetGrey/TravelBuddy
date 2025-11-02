namespace TravelBuddy.Users.DTOs
{
    public record AuthResponseDto(
        UserDto user,
        string Token
    );
}