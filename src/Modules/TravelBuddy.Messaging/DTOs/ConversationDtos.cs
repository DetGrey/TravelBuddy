namespace TravelBuddy.Messaging;

public record ConversationSummaryDto (
    int Id,
    bool IsGroup,
    bool IsArchived,
    int ParticipantCount,
    DateTime? CreatedAt
);

public record ConversationParticipantDto (
    int UserId,
    string Name,
    string Email
);

public record ConversationDetailDto (
    int Id,
    bool IsGroup,
    bool IsArchived,
    DateTime? CreatedAt,
    int ParticipantCount, 
    IEnumerable<ConversationParticipantDto> Participant
);