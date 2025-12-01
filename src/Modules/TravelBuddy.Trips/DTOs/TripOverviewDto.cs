namespace TravelBuddy.Trips.DTOs
{
    public record TripOverviewDto(
        int TripId,
        string TripName,
        DateOnly TripStartDate,
        DateOnly TripEndDate,
        int MaxBuddies,
        string TripDescription,
        int OwnerUserId,
        string OwnerName,
        List<SimplifiedTripDestinationDto> Destinations
    );

    public record SimplifiedTripDestinationDto(
        int TripDestinationId,
        int TripId,
        DateOnly DestinationStartDate,
        DateOnly DestinationEndDate,
        string DestinationName,
        string? DestinationState,
        string DestinationCountry,
        int MaxBuddies,
        int AcceptedBuddiesCount
    );
}
