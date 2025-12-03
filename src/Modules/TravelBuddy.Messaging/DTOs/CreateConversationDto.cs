namespace TravelBuddy.Messaging;

public record CreateConversationDto (
    int OwnerId,
    int? TripDestinationId,
    bool IsGroup,
    int? OtherUserId
);