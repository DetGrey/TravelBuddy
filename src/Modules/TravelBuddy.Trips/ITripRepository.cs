using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
public interface ITripRepository
{
    Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
        DateOnly? reqStart,
        DateOnly? reqEnd,
        string? country,
        string? state,
        string? name,
        int? partySize,
        string? q
    );
    Task<IEnumerable<Destination>> GetDestinationsAsync();
    Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId);
    Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId);
    Task<TripOverview?> GetFullTripOverviewAsync(int tripId);
    Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId);
    Task<int?> GetTripOwnerAsync(int tripDestinationId);
    Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto);

    // Buddy related
    Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int changedBy, string departureReason);
    Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId);
    Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto);
    Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto);

    // Admin deletion methods
    Task<(bool Success, string? ErrorMessage)> DeleteTripAsync(int tripId, int changedBy);
    Task<(bool Success, string? ErrorMessage)> DeleteTripDestinationAsync(int tripDestinationId, int changedBy);
    Task<(bool Success, string? ErrorMessage)> DeleteDestinationAsync(int destinationId, int changedBy);

    // Owner update methods
    Task<(bool Success, string? ErrorMessage)> UpdateTripInfoAsync(int tripId, int ownerId, string? tripName, string? description);
    Task<(bool Success, string? ErrorMessage)> UpdateTripDestinationDescriptionAsync(int tripDestinationId, int ownerId, string? description);

    // Audit related
    Task<IEnumerable<TripAudit>> GetTripAuditsAsync();
    Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync();
}
