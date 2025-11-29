using Neo4j.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Trips;
// CLASS
public class Neo4jBuddyRepository : IBuddyRepository
{
    private readonly IDriver _driver;

    public Neo4jBuddyRepository(IDriver driver)
    {
        _driver = driver;
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