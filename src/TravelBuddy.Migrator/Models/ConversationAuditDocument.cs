using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class ConversationAuditDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int AuditId { get; set; }

    public int ConversationId { get; set; }

    public int? AffectedUserId { get; set; }

    public string Action { get; set; } = null!;

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
