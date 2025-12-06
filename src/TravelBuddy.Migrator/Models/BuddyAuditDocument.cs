using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class BuddyAuditDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int AuditId { get; set; }

    public int BuddyId { get; set; }

    public string Action { get; set; } = null!;

    public string? Reason { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
