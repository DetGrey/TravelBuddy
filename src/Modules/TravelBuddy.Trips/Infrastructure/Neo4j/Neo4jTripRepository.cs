using Neo4j.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
// CLASS
public class Neo4jTripRepository : ITripRepository
{
    private readonly IDriver _driver;

    public Neo4jTripRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
        DateOnly? reqStart,
        DateOnly? reqEnd,
        string? country,
        string? state,
        string? name,
        int? partySize,
        string? q)
    {
        // TODO Placeholder: Return an empty collection of search results
        return await Task.FromResult<IEnumerable<TripDestinationSearchResult>>(new List<TripDestinationSearchResult>());
    }

    public async Task<IEnumerable<Destination>> GetDestinationsAsync()
    {
        // TODO Placeholder: Return an empty collection of destinations
        return await Task.FromResult<IEnumerable<Destination>>(new List<Destination>());
    }

    public async Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId)
    {
        // TODO Placeholder: Return an empty list of user trip summaries
        return await Task.FromResult<IEnumerable<BuddyTripSummary>>(new List<BuddyTripSummary>());
    }

    public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
    {
        // TODO Placeholder: Return null, indicating no trip destination info found
        return await Task.FromResult<TripDestinationInfo?>(null);
    }
    public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
    {
        // TODO Placeholder: Return null, indicating no trip overview found
        return await Task.FromResult<TripOverview?>(null);
    }
    public async Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId)
    {
        // TODO Placeholder: Return an empty list of trip overviews
        return await Task.FromResult<List<TripOverview>>(new List<TripOverview>());
    }

    public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
    {
        // TODO Placeholder: Return null, indicating the owner ID isn't found
        return await Task.FromResult<int?>(null);
    }

    // ---------------------------------------------------------
    // Create a new trip with destinations
    // ---------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto)
    {
        // TODO implement
        return await Task.FromResult((false, "Not implemented"));
    }

    // -----------------------------------------------------------------------------------------------
    // Buddy related
    public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
        int userId,
        int tripDestinationId,
        int triggeredBy,
        string departureReason
    )
    {
        // TODO Placeholder: Return a successful result tuple with no error message
        return await Task.FromResult((Success: true, ErrorMessage: (string?)null));
    }
    public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
    {
        // TODO Placeholder: Return an empty list of PendingBuddyRequest
        return await Task.FromResult<IEnumerable<PendingBuddyRequest>>(new List<PendingBuddyRequest>());
    }

    public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        // TODO Placeholder: Return true to simulate a successful insertion
        return await Task.FromResult<(bool, string?)>((true, null));
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        // TODO Placeholder: Return false to simulate a failed or neutral update (can be true if needed)
        return await Task.FromResult<(bool, string?)>((true, null));
    }
}
