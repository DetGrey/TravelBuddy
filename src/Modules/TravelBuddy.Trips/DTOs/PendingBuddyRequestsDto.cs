namespace TravelBuddy.Trips.DTOs
{
    public record PendingBuddyRequestsDto(
        int TripId,
        string DestinationName,
        int BuddyId,
        int UserId,
        string BuddyName,
        string? BuddyNote,
        int PersonCount
    );
}