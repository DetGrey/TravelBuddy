using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class PasswordChangeRequestDto
    {
        [Required]
        public string OldPassword { get; set; } = null!;
        [Required]
        public string NewPassword { get; set; } = null!;
    }
}