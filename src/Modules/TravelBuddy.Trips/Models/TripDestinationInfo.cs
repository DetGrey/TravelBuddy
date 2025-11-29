namespace TravelBuddy.Trips.Models
{
    public class TripDestinationInfo
    {
        public int TripDestinationId { get; set; }
        public DateOnly DestinationStartDate { get; set; }
        public DateOnly DestinationEndDate { get; set; }
        public string? DestinationDescription { get; set; }
        public bool? DestinationIsArchived { get; set; }

        // Trip info
        public int TripId { get; set; }
        public int? MaxBuddies { get; set; }

        // Destination info
        public int DestinationId { get; set; }
        public string DestinationName { get; set; } = null!;
        public string? DestinationState { get; set; }
        public string DestinationCountry { get; set; } = null!;
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }

        // Owner info
        public int OwnerUserId { get; set; }
        public string OwnerName { get; set; } = null!;

        // Group conversation info
        public int? GroupConversationId { get; set; }

        // Buddies
        public List<BuddyInfo> AcceptedBuddies { get; set; } = new();
        public List<BuddyRequestInfo> PendingRequests { get; set; } = new();
    }

    public class BuddyInfo
    {
        public int BuddyId { get; set; }
        public int PersonCount { get; set; }
        public string? BuddyNote { get; set; }

        // User info
        public int BuddyUserId { get; set; }
        public string BuddyName { get; set; } = null!;
    }

    public class BuddyRequestInfo
    {
        public int BuddyId { get; set; }
        public int PersonCount { get; set; }
        public string? BuddyNote { get; set; }

        // Requester info
        public int RequesterUserId { get; set; }
        public string RequesterName { get; set; } = null!;
    }
}