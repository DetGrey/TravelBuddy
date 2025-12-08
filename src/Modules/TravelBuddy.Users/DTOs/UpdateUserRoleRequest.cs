using System.ComponentModel.DataAnnotations;

namespace TravelBuddy.Users.DTOs
{
    public class UpdateUserRoleRequest
    {
        [Required]
        [RegularExpression(@"^(user|admin)$", ErrorMessage = "Role must be either 'user' or 'admin'.")]
        public string Role { get; set; } = null!;
    }
}
