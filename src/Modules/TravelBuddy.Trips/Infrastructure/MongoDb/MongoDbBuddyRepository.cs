using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips;
public class MongoDbBuddyRepository : IBuddyRepository
{
    // TODO since this is part of a trip how does that work?
    private readonly IMongoCollection<Trip> _tripsCollection; // TODO update if not Trip

    public MongoDbBuddyRepository(IMongoClient client)
    {
        var database = client.GetDatabase("travel_buddy_mongo");
        _tripsCollection = database.GetCollection<Trip>("trips"); //TODO update
    }

    public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
    {
        // TODO Placeholder: Return an empty list of PendingBuddyRequest
        return await Task.FromResult<IEnumerable<PendingBuddyRequest>>(new List<PendingBuddyRequest>());
    }

    public async Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        // TODO Placeholder: Return true to simulate a successful insertion
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        // TODO Placeholder: Return false to simulate a failed or neutral update (can be true if needed)
        return await Task.FromResult(false);
    }
}