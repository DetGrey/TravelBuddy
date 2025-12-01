namespace TravelBuddy.Trips.DTOs
{
     public record TripDestinationSearchDto(
        int TripDestinationId,
        int TripId,
        int DestinationId,
        string DestinationName,
        string Country,
        string? State,
        DateOnly DestinationStart,
        DateOnly DestinationEnd,
        int MaxBuddies,
        int AcceptedPersons,
        int RemainingCapacity
    );
}