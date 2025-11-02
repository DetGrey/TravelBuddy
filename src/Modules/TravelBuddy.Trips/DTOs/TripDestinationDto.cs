namespace TravelBuddy.Trips.DTOs
{
    // DTO (Data Transfer Object)
    public record TripDestinationDto(
        int TripDestinationId,
        int DestinationId,
        int TripId,
        DateOnly StartDate,
        DateOnly EndDate,
        int SequenceNumber,
        string? Description,
        bool? IsArchived
    );
}