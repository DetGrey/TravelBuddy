using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class MessageDocument
{
    [BsonId]
    public int MessageId { get; set; }

    public int ConversationId { get; set; }
    public int SenderId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime? SentAt { get; set; }
}
