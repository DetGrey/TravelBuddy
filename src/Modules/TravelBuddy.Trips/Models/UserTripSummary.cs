namespace TravelBuddy.Trips.Models
{
    public class UserTripSummary
    {
        public int TripId { get; set; }
        public int TripDestinationId { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public string TripDescription { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "owner" or "buddy"
    }
}