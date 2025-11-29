namespace TravelBuddy.Trips.Models
{
    public class PendingBuddyRequest
    {
        public int TripDestinationId { get; set; }
        public string DestinationName { get; set; } = string.Empty;
        public DateOnly DestinationStartDate { get; set; }
        public DateOnly DestinationEndDate { get; set; }
        public int TripId { get; set; }
        public int BuddyId { get; set; }
        public int RequesterUserId { get; set; }
        public string RequesterName { get; set; } = string.Empty;
        public string? BuddyNote { get; set; }
        public int PersonCount { get; set; }
    }
}