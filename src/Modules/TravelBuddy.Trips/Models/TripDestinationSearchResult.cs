namespace TravelBuddy.Trips.Models
{
    public class TripDestinationSearchResult
    {
        public int TripDestinationId { get; set; }
        public int TripId { get; set; }
        public int DestinationId { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? State { get; set; } = string.Empty;
        public DateOnly DestinationStart { get; set; }
        public DateOnly DestinationEnd { get; set; }
        public int MaxBuddies { get; set; }
        public int AcceptedPersons { get; set; }
        public int RemainingCapacity { get; set; }
    }
}