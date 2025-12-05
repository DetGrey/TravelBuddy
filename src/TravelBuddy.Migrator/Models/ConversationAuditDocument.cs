namespace TravelBuddy.Migrator.Models;

public class ConversationAuditDocument
{
    public int AuditId { get; set; }

    public int ConversationId { get; set; }

    public int? AffectedUserId { get; set; }

    public string Action { get; set; } = null!;

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
