using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class ChangePasswordRequestDto
    {
        [Required]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "Old password cannot be empty or whitespace.")]
        [MinLength(6)]
        public string OldPassword { get; set; } = null!;
        [Required]
        [RegularExpression(@"^.*\S.*$", ErrorMessage = "New password cannot be empty or whitespace.")]
        [MinLength(6)]
        public string NewPassword { get; set; } = null!;
    }
}