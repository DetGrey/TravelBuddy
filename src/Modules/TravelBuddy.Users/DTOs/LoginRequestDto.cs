using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}