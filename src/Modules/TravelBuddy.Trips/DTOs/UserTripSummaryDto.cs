namespace TravelBuddy.Trips.DTOs
{
    public record UserTripSummaryDto(
        int TripId,
        int TripDestinationId,
        string DestinationName,
        string TripDescription,
        DateOnly StartDate,
        DateOnly EndDate,
        bool IsArchived,
        string Role
    );
}