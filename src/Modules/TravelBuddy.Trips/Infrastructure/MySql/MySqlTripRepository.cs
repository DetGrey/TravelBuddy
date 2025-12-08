using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips;
// CLASS
public class MySqlTripRepository : ITripRepository
{
    private readonly TripsDbContext _context;

    public MySqlTripRepository(TripsDbContext context)
    {
        _context = context;
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
        var start = reqStart.HasValue ? reqStart.Value : (DateOnly?)null;
        var end = reqEnd.HasValue ? reqEnd.Value : (DateOnly?)null;
        var countryParam = country ?? string.Empty;
        var stateParam = state ?? string.Empty;
        var nameParam = name ?? string.Empty;
        var partySizeParam = partySize ?? 0;
        var qParam = q ?? string.Empty;

        return await _context.Set<TripDestinationSearchResult>()
            .FromSqlInterpolated($@"
                CALL search_trips(
                    {start},
                    {end},
                    {countryParam},
                    {stateParam},
                    {nameParam},
                    {partySizeParam},
                    {qParam}
                )")
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<IEnumerable<Destination>> GetDestinationsAsync()
    {
        return await _context.Destinations
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId)
    {
        return await _context.BuddyTripSummaries
            .FromSqlInterpolated($@"
                CALL get_buddy_trips({userId})") // FromSqlInterpolated() automatically prevents SQL injections
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
    {
        // Ensure TripDestinationInfo is configured to map to V_TripDestinationInfo
        // Use standard LINQ to apply the WHERE clause asynchronously.
        var tripDestination = await _context.TripDestinationInfo
            .FromSqlInterpolated($"SELECT * FROM V_TripDestinationInfo WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (tripDestination == null) return null;

        // Load Accepted Buddies
        // Ensure BuddyInfo is configured to map to V_AcceptedBuddies
        var acceptedBuddies = await _context.BuddyInfo
            .FromSqlInterpolated($"SELECT * FROM V_AcceptedBuddies WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .ToListAsync();

        // Load Pending Requests
        // Ensure BuddyRequestInfo is configured to map to V_PendingRequests
        var pendingRequests = await _context.BuddyRequestInfo
            .FromSqlInterpolated($"SELECT * FROM V_PendingRequests WHERE TripDestinationId = {tripDestinationId}")
            .AsNoTracking()
            .ToListAsync();
            
        // Populate the lists
        tripDestination.AcceptedBuddies.AddRange(acceptedBuddies);
        tripDestination.PendingRequests.AddRange(pendingRequests);

        return tripDestination;
    }
    private async Task<List<SimplifiedTripDestination>> GetTripDestinationsWithCountsAsync(int tripId)
    {
        var destinations = await _context.SimplifiedTripDestination
            .FromSqlInterpolated($"SELECT * FROM V_SimplifiedTripDest WHERE TripId = {tripId}")
            .AsNoTracking()
            .ToListAsync();

        if (!destinations.Any())
        {
            return new List<SimplifiedTripDestination>();
        }

        var destinationIds = destinations.Select(d => d.TripDestinationId).ToList();

        var buddyCounts = await _context.Buddies
            .Where(b => destinationIds.Contains(b.TripDestinationId) &&
                        b.RequestStatus == "accepted" &&
                        b.IsActive == true)
            .GroupBy(b => b.TripDestinationId)
            .Select(g => new
            {
                TripDestinationId = g.Key,
                AcceptedCount = g.Sum(b => b.PersonCount)
            })
            .ToListAsync();
            
        var countsDictionary = buddyCounts.ToDictionary(x => x.TripDestinationId, x => x.AcceptedCount);

        foreach (var dest in destinations)
        {
            if (countsDictionary.TryGetValue(dest.TripDestinationId, out var count))
            {
                dest.AcceptedBuddiesCount = count ?? 0;
            }
            else
            {
                dest.AcceptedBuddiesCount = 0;
            }
        }

        return destinations;
    }
    public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
    {
        var tripHeader = await _context.TripHeaderInfo
            .FromSqlInterpolated($"SELECT * FROM V_TripHeaderInfo WHERE TripId = {tripId}")
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (tripHeader == null) return null;
        
        var destinationsWithCounts = await GetTripDestinationsWithCountsAsync(tripId);

        var tripOverview = new TripOverview
        {
            // Map Header Properties
            TripId = tripHeader.TripId,
            TripName = tripHeader.TripName,
            TripStartDate = tripHeader.TripStartDate,
            TripEndDate = tripHeader.TripEndDate,
            MaxBuddies = tripHeader.MaxBuddies,
            TripDescription = tripHeader.TripDescription,
            OwnerUserId = tripHeader.OwnerUserId,
            OwnerName = tripHeader.OwnerName,
            
            // Assign the enriched destination list
            Destinations = destinationsWithCounts
        };

        return tripOverview;
    }
    public async Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId)
    {
        var tripHeaders = await _context.TripHeaderInfo
            .FromSqlInterpolated($"SELECT * FROM V_TripHeaderInfo WHERE OwnerUserId = {userId}")
            .AsNoTracking()
            .ToListAsync();

        if (!tripHeaders.Any()) return new List<TripOverview>();

        // Fetch destinations for all trips in one go
        var destinationsLookup = await GetTripsDestinationsWithCountsAsync(tripHeaders.Select(t => t.TripId).ToList());

        var tripOverviews = tripHeaders.Select(tripHeader => new TripOverview
        {
            TripId = tripHeader.TripId,
            TripName = tripHeader.TripName,
            TripStartDate = tripHeader.TripStartDate,
            TripEndDate = tripHeader.TripEndDate,
            MaxBuddies = tripHeader.MaxBuddies,
            TripDescription = tripHeader.TripDescription,
            OwnerUserId = tripHeader.OwnerUserId,
            OwnerName = tripHeader.OwnerName,
            Destinations = destinationsLookup.TryGetValue(tripHeader.TripId, out var destinations)
                ? destinations
                : new List<SimplifiedTripDestination>()
        }).ToList();

        return tripOverviews;
    }
    private async Task<Dictionary<int, List<SimplifiedTripDestination>>> GetTripsDestinationsWithCountsAsync(List<int> tripIds)
    {
        // Fetch all destinations for the given trips in one query
        var destinations = await _context.SimplifiedTripDestination
            .Where(d => tripIds.Contains(d.TripId))
            .AsNoTracking()
            .ToListAsync();

        if (!destinations.Any())
            return new Dictionary<int, List<SimplifiedTripDestination>>();

        // Collect all destination IDs across trips
        var destinationIds = destinations.Select(d => d.TripDestinationId).ToList();

        // Fetch buddy counts for all destinations in one query
        var buddyCounts = await _context.Buddies
            .Where(b => destinationIds.Contains(b.TripDestinationId) &&
                        b.RequestStatus == "accepted" &&
                        b.IsActive == true)
            .GroupBy(b => b.TripDestinationId)
            .Select(g => new
            {
                TripDestinationId = g.Key,
                AcceptedCount = g.Sum(b => b.PersonCount)
            })
            .ToListAsync();

        var countsDictionary = buddyCounts.ToDictionary(x => x.TripDestinationId, x => x.AcceptedCount);

        // Enrich each destination with its buddy count
        foreach (var dest in destinations)
        {
            dest.AcceptedBuddiesCount = countsDictionary.TryGetValue(dest.TripDestinationId, out var count) 
                ? (count ?? 0) 
                : 0;
        }

        // Group destinations back by TripId
        return destinations
            .GroupBy(d => d.TripId)
            .ToDictionary(g => g.Key, g => g.ToList());
    }
    public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
    {
        // SqlQuery is better than FromSqlInterpolated when it returns only one value
        return await _context.Database
            .SqlQuery<int>($@"
                SELECT t.owner_id AS Value
                FROM trips t
                JOIN trip_destinations td ON t.trip_id = td.trip_id
                WHERE td.trip_destination_id = {tripDestinationId}
                LIMIT 1")
            .SingleOrDefaultAsync();
    }

    public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto)
    {   
        // SHow a list of all lat long
        Console.WriteLine($"Long and lats: {string.Join(", ", createTripWithDestinationsDto.TripDestinations.Select(td => $"({td.Longitude}, {td.Latitude})"))}");
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Create Trip
            var (tripSuccess, tripError, newTripId) = await CreateTripAsync(createTripWithDestinationsDto.CreateTrip);
            if (!tripSuccess || newTripId == null)
            {
                await transaction.RollbackAsync();
                return (false, tripError);
            }

            var tripDestinationsWithId = createTripWithDestinationsDto.TripDestinations.Select(td => {
                td.TripId = newTripId.Value;
                return td;
            }).ToList();

            if (!tripDestinationsWithId.Any())
            {
                await transaction.RollbackAsync();
                return (false, "At least one trip destination is required.");
            }
            // Create Trip Destinations
            var (destSuccess, destError) = await CreateTripDestinationsAsync(tripDestinationsWithId);
            if (!destSuccess)
            {
                await transaction.RollbackAsync();
                return (false, destError);
            }

            await transaction.CommitAsync();
            return (true, null);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, "Error: " + ex.Message);
        }
    }
    private async Task<(bool Success, string? ErrorMessage, int? NewTripId)> CreateTripAsync(CreateTripDto trip)
    {
        try
        {
            var result = await _context.NewTripIds.FromSqlInterpolated($@"
                CALL create_trip(
                    {trip.OwnerId},
                    {trip.TripName},
                    {trip.MaxBuddies},
                    {trip.StartDate},
                    {trip.EndDate},
                    {trip.Description},
                    {trip.ChangedBy}
                )")
                .ToListAsync();

            int newTripId = result.FirstOrDefault()?.TripId ?? 0;

            if (newTripId > 0)
            {
                return (true, null, newTripId);
            }
            
            return (false, "Trip creation succeeded but ID was not returned.", null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message, null);
        }
    }

    private async Task<(bool Success, string? ErrorMessage)> CreateTripDestinationsAsync(IEnumerable<CreateTripDestinationDto> tripDestinations)
    {
        try
        {
            foreach (var dest in tripDestinations)
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    CALL create_trip_destination(
                        {dest.TripId},
                        {dest.DestinationId},
                        {dest.Name},
                        {dest.State},
                        {dest.Country},
                        {dest.Longitude},
                        {dest.Latitude},
                        {dest.DestinationStartDate},
                        {dest.DestinationEndDate},
                        {dest.SequenceNumber},
                        {dest.Description}
                    )");    
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    // ----------------------------------------------------------------------------------
    // BUDDY RELATED METHODS
    // ----------------------------------------------------------------------------------
    public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
        int userId,
        int tripDestinationId,
        int changedBy,
        string departureReason
    )
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL remove_buddy_from_trip_destination(
                    {userId},
                    {tripDestinationId},
                    {changedBy},
                    {departureReason}
                )");

            return (true, null); // Success
        }
        catch (Exception ex)
        {
            // For general debugging, you can return the full exception message
            // which usually contains the database error details.
            // If you're using a specific provider (like Npgsql for PostgreSQL 
            // or SqlClient for SQL Server), you might need to cast 'ex' 
            // to get the specific database exception type (e.g., PostgresException) 
            // for more granular error codes/messages.

            return (false, "Error: " + ex.Message); // Failure with error message
        }
    }

    public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
    {
        return await _context.PendingBuddyRequests
            .FromSqlInterpolated($@"
                CALL get_pending_buddy_requests({userId})")
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
    {
        try
        {
            var noteParam = new MySqlParameter { 
                MySqlDbType = MySqlDbType.VarChar, 
                Size = 255, 
                Value = (object?)buddyDto.Note ?? DBNull.Value 
            };

            await _context.Database.ExecuteSqlRawAsync(
                "CALL request_to_join_trip_destination({0}, {1}, {2}, {3})",
                buddyDto.UserId,
                buddyDto.TripDestinationId,
                buddyDto.PersonCount,
                noteParam
            );

            return (true, null);
        }
        catch (Exception ex)
        {
            // If it's a MySqlException, get the real message
            if (ex is MySqlException mysqlEx)
            {
                return (false, "Error: " + mysqlEx.Message);
            }

            // Otherwise fall back to generic
            return (false, "Error: " + ex.Message);
        }
    }
    public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_buddy_request_tx(
                    {updateBuddyRequestDto.BuddyId},
                    {updateBuddyRequestDto.UserId},
                    {updateBuddyRequestDto.NewStatus.ToString().ToLower()}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
             return (false, "Error: " + ex.Message); 
        }
    }

    // ------------------------------- ADMIN DELETION METHODS -------------------------------
    public async Task<(bool Success, string? ErrorMessage)> DeleteTripAsync(int tripId, int changedBy)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL delete_trip(
                    {tripId},
                    {changedBy}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteTripDestinationAsync(int tripDestinationId, int changedBy)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL delete_trip_destination(
                    {tripDestinationId},
                    {changedBy}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteDestinationAsync(int destinationId, int changedBy)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL admin_delete_destination(
                    {destinationId},
                    {changedBy}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateTripInfoAsync(int tripId, int ownerId, string? tripName, string? description)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_trip_info(
                    {tripId},
                    {ownerId},
                    {tripName},
                    {description}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateTripDestinationDescriptionAsync(int tripDestinationId, int ownerId, string? description)
    {
        try
        {
            await _context.Database.ExecuteSqlInterpolatedAsync($@"
                CALL update_trip_destination_description(
                    {tripDestinationId},
                    {ownerId},
                    {description}
                )");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, "Error: " + ex.Message);
        }
    }

    // ------------------------------- AUDIT TABLES -------------------------------
    public async Task<IEnumerable<TripAudit>> GetTripAuditsAsync()
    {
        return await _context.TripAudits
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync()
    {
        return await _context.BuddyAudits
            .AsNoTracking()
            .ToListAsync();
    }
}
