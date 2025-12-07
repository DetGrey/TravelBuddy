using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(1)]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "Name cannot be empty or whitespace.")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = null!;
        [Required]
        [EmailAddress]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "Email cannot be empty or whitespace.")]
        [MinLength(6)]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; } = null!;
        [Required]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "Password cannot be empty or whitespace.")]
        [MinLength(6)]
        public string Password { get; set; } = null!;
        [Required]
        public DateOnly? Birthdate { get; set; }
    }
}