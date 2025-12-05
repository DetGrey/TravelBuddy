namespace TravelBuddy.Migrator.Models;

public class UserAuditDocument
{
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? FieldChanged { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
