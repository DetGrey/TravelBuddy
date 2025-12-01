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
    Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int triggeredBy, string departureReason);
    Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId);
    Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto);
    Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto);

    // Audit related
    Task<IEnumerable<TripAudit>> GetTripAuditsAsync();
    Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync();
}
