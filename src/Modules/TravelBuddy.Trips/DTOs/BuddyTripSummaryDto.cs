namespace TravelBuddy.Trips.DTOs
{
    public record BuddyTripSummaryDto(
        int TripId,
        int TripDestinationId,
        string DestinationName,
        string TripDescription,
        DateOnly StartDate,
        DateOnly EndDate,
        bool IsArchived
    );
}