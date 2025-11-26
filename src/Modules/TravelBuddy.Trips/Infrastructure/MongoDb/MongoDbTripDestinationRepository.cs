using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
public class MongoDbTripDestinationRepository : ITripDestinationRepository
{
    private readonly IMongoCollection<Trip> _tripsCollection; // TODO update if not Trip

    public MongoDbTripDestinationRepository(IMongoClient client)
    {
        var database = client.GetDatabase("travel_buddy_mongo");
        _tripsCollection = database.GetCollection<Trip>("trips"); //TODO update
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

    public async Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId)
    {
        // TODO Placeholder: Return an empty list of user trip summaries
        return await Task.FromResult<IEnumerable<UserTripSummary>>(new List<UserTripSummary>());
    }

    public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
    {
        // TODO Placeholder: Return null, indicating the owner ID isn't found
        return await Task.FromResult<int?>(null);
    }

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
}
