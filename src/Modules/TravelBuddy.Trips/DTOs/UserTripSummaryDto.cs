namespace TravelBuddy.Trips.DTOs
{
    public record UserTripSummaryDto(
        int TripId,
        int TripDestinationId,
        string DestinationName,
        string TripDescription,
        string Role
    );
}