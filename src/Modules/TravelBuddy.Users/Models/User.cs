namespace TravelBuddy.Users.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public bool? IsDeleted { get; set; }

    public string? Role { get; set; }
}
