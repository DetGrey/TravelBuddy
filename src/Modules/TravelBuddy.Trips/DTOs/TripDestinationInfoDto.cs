namespace TravelBuddy.Trips.DTOs
{
    public record TripDestinationInfoDto(
        int TripDestinationId,
        DateOnly DestinationStartDate,
        DateOnly DestinationEndDate,
        string? DestinationDescription,
        bool? DestinationIsArchived,
        int TripId,
        int? MaxBuddies,
        int DestinationId,
        string DestinationName,
        string? DestinationState,
        string DestinationCountry,
        decimal? Longitude,
        decimal? Latitude,
        int OwnerUserId,
        string OwnerName,
        int? GroupConversationId,
        IEnumerable<BuddyInfoDto> AcceptedBuddies,
        IEnumerable<BuddyRequestInfoDto> PendingRequests
    );
    public record BuddyInfoDto(
        int BuddyId,
        int PersonCount,
        string? BuddyNote,
        int BuddyUserId,
        string BuddyName
    );
    public record BuddyRequestInfoDto(
        int BuddyId,
        int PersonCount,
        string? BuddyNote,
        int RequesterUserId,
        string RequesterName
    );
}