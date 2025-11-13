namespace TravelBuddy.Messaging;

public record ConversationSummaryDto (
    int Id,
    bool IsGroup,
    bool IsArchived,
    int ParticipantCount,
    DateTime? CreatedAt
);