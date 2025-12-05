namespace TravelBuddy.Migrator.Models;

public class BuddyAuditDocument
{
    public int AuditId { get; set; }

    public int BuddyId { get; set; }

    public string Action { get; set; } = null!;

    public string? Reason { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
