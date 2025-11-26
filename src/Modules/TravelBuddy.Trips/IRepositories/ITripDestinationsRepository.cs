using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
public interface ITripDestinationRepository
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

    Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId);
    Task<int?> GetTripOwnerAsync(int tripDestinationId);
    Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(int userId, int tripDestinationId, int triggeredBy, string departureReason);
}
