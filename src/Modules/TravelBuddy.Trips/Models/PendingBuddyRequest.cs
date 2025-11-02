namespace TravelBuddy.Trips.Models
{
    public class PendingBuddyRequest
    {
        public int TripId { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public int BuddyUserId { get; set; }
        public string BuddyName { get; set; } = string.Empty;
        public string? BuddyNote { get; set; }
        public int PersonCount { get; set; }
    }
}