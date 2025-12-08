using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class SystemEventLogDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int EventId { get; set; }

    public string EventType { get; set; } = null!;

    public int? AffectedId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Details { get; set; }
}
