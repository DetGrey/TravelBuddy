namespace TravelBuddy.Users.Models;

public partial class UserAudit
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? FieldChanged { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? ChangedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
