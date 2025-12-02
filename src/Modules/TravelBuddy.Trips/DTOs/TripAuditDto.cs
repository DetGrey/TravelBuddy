namespace TravelBuddy.Trips.DTOs;

public record TripAuditDto(
    int AuditId,
    int TripId,
    string Action,
    string? FieldChanged,
    string? OldValue,
    string? NewValue,
    int? ChangedBy,
    DateTime? Timestamp
);
