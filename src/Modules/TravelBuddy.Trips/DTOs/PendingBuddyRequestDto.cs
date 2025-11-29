namespace TravelBuddy.Trips.DTOs
{
    public record PendingBuddyRequestDto(
        int TripDestinationId,
        string DestinationName,
        DateOnly DestinationStartDate,
        DateOnly DestinationEndDate,
        int TripId,
        int BuddyId,
        int RequesterUserId,
        string RequesterName,
        string? BuddyNote,
        int PersonCount
    );
}