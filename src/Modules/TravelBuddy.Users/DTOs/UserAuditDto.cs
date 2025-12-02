namespace TravelBuddy.Users.Models;

public record UserAuditDto(
    int AuditId,
    int UserId,
    string Action,
    string? FieldChanged,
    string? OldValue,
    string? NewValue,
    int? ChangedBy,
    DateTime? Timestamp
);
