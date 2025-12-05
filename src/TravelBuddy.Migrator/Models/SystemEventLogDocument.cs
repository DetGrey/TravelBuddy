namespace TravelBuddy.Migrator.Models;

public class SystemEventLogDocument
{
    public int EventId { get; set; }

    public string EventType { get; set; } = null!;

    public int? AffectedId { get; set; }

    public DateTime? TriggeredAt { get; set; }

    public string? Details { get; set; }
}
