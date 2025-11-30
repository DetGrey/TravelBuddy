namespace TravelBuddy.Messaging.Models;

public class ConversationOverview
{
    public int ConversationId { get; set; }
    public int? TripDestinationId { get; set; }
    public bool IsGroup { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool IsArchived { get; set; }
    public int ParticipantCount { get; set; }
    public string? LastMessagePreview { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public string? ConversationName { get; set; }
}
