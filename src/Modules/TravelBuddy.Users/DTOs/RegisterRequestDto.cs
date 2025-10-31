namespace TravelBuddy.Users.DTOs
{
    public class RegisterRequestDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateOnly Birthdate { get; set; }
    }
}