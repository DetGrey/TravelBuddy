using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class UserAuditDocument
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.Int32)]
    public int AuditId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? FieldChanged { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }
}
