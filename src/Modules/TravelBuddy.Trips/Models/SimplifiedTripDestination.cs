namespace TravelBuddy.Trips.Models;

public class SimplifiedTripDestination
{
    public int TripDestinationId { get; set; }
    public int TripId { get; set; }
    public DateOnly DestinationStartDate { get; set; }
    public DateOnly DestinationEndDate { get; set; }
    public string DestinationName { get; set; } = null!;
    public string? DestinationState { get; set; } = null!;
    public string DestinationCountry { get; set; } = null!;
    public int MaxBuddies { get; set; }

    // This property will be populated manually after the second query
    public int AcceptedBuddiesCount { get; set; }
}
