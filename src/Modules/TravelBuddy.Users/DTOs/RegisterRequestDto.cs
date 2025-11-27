using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public DateOnly Birthdate { get; set; }
    }
}