using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class ConversationDocument
{
    [BsonId]
    public int ConversationId { get; set; }

    public int? TripDestinationId { get; set; }
    public bool IsGroup { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool IsArchived { get; set; }

    public List<ConversationParticipantEmbedded> Participants { get; set; } = new();
}

public class ConversationParticipantEmbedded
{
    public int UserId { get; set; }
    public DateTime? JoinedAt { get; set; }
}
