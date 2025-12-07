using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        [MinLength(6)]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = null!;
    }
}