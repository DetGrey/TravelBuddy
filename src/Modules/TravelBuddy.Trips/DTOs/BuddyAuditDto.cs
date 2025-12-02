namespace TravelBuddy.Trips.DTOs;

public record BuddyAuditDto
(
    int AuditId,
    int BuddyId,
    string Action,
    string? Reason,
    int? ChangedBy,
    DateTime? Timestamp
);