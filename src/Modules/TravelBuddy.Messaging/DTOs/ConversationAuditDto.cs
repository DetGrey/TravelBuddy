namespace TravelBuddy.Messaging.Models;

public record ConversationAuditDto(
    int AuditId,
    int ConversationId,
    int? AffectedUserId,
    string Action,
    int? ChangedBy,
    DateTime? Timestamp
);
