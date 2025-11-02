namespace TravelBuddy.Trips.DTOs
{
    public record PendingBuddyRequestsDto(
        int TripId,
        string DestinationName,
        int BuddyUserId,
        string BuddyName,
        string? BuddyNote,
        int PersonCount
    );
}