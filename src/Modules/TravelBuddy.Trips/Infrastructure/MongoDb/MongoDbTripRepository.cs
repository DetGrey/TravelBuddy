using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips
{
    // ---------- Mongo document shapes (match migrator) ----------
    [BsonIgnoreExtraElements]
    internal class UserDocument
    {
        [BsonElement("UserId")]
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    [BsonIgnoreExtraElements]
    internal class BuddyEmbedded
    {
        public int BuddyId { get; set; }
        public int UserId { get; set; }
        public int? PersonCount { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? DepartureReason { get; set; }
        public string RequestStatus { get; set; } = string.Empty; // "pending", "accepted", "rejected"
    }

    [BsonIgnoreExtraElements]
    internal class TripDestinationEmbedded
    {
        public int TripDestinationId { get; set; }
        public int DestinationId { get; set; }
        public DateOnly StartDate { get; set; }   // stored as DateOnly in Mongo
        public DateOnly EndDate { get; set; }
        public int SequenceNumber { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }
        public List<BuddyEmbedded> Buddies { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class TripDocument
    {
        // We do NOT map _id; we keep TripId as a normal field
        public int TripId { get; set; }
        public int? OwnerId { get; set; }
        public int? MaxBuddies { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }

        public List<TripDestinationEmbedded> Destinations { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class DestinationDocument
    {
        public int DestinationId { get; set; }
        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }

    // ---------- Repository implementation ----------

    public class MongoDbTripRepository : ITripRepository
    {
        private readonly IMongoCollection<TripDocument> _tripsCollection;
        private readonly IMongoCollection<DestinationDocument> _destinationsCollection;
        private readonly IMongoCollection<UserDocument> _usersCollection;

        public MongoDbTripRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _tripsCollection = database.GetCollection<TripDocument>("trips");
            _destinationsCollection = database.GetCollection<DestinationDocument>("destinations");
            _usersCollection = database.GetCollection<UserDocument>("users");
        }

        // Helper to load all trips and destinations into memory
        private async Task<(List<TripDocument> Trips, Dictionary<int, DestinationDocument> Destinations)>
            LoadTripsAndDestinationsAsync()
        {
            var tripsTask = _tripsCollection
                .Find(FilterDefinition<TripDocument>.Empty)
                .ToListAsync();

            var destsTask = _destinationsCollection
                .Find(FilterDefinition<DestinationDocument>.Empty)
                .ToListAsync();

            await Task.WhenAll(tripsTask, destsTask);

            var trips = tripsTask.Result;

            // Some destination documents may share the same DestinationId (e.g., 0)
            // so we group by DestinationId and keep only the first for each key.
            var destMap = destsTask.Result
                .GroupBy(d => d.DestinationId)
                .ToDictionary(g => g.Key, g => g.First());

            return (trips, destMap);
        }

        // Helper: load all trips into memory
        private async Task<List<TripDocument>> LoadTripsAsync()
        {
            return await _tripsCollection
                .Find(FilterDefinition<TripDocument>.Empty)
                .ToListAsync();
        }

        // Helper: generate next BuddyId (global)
        private async Task<int> GetNextBuddyIdAsync()
        {
            var trips = await LoadTripsAsync();
            var max = trips
                .SelectMany(t => t.Destinations)
                .SelectMany(td => td.Buddies)
                .DefaultIfEmpty()
                .Max(b => b?.BuddyId ?? 0);

            return max + 1;
        }

        // ---------------------------------------------------------
        // Search available trips / destinations
        // ---------------------------------------------------------
        public async Task<IEnumerable<TripDestinationSearchResult>> SearchTripsAsync(
            DateOnly? reqStart,
            DateOnly? reqEnd,
            string? country,
            string? state,
            string? name,
            int? partySize,
            string? q)
        {
            var (trips, destMap) = await LoadTripsAndDestinationsAsync();

            var results = new List<TripDestinationSearchResult>();

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    var destStart = td.StartDate;
                    var destEnd = td.EndDate;

                    // --- date overlap filter ---
                    if (reqStart.HasValue && destEnd < reqStart.Value)
                        continue;
                    if (reqEnd.HasValue && destStart > reqEnd.Value)
                        continue;

                    // --- country/state filter (case-insensitive) ---
                    if (!string.IsNullOrWhiteSpace(country) &&
                        !string.Equals(destDoc.Country, country, StringComparison.OrdinalIgnoreCase))
                        continue;

                    if (!string.IsNullOrWhiteSpace(state) &&
                        !string.Equals(destDoc.State ?? string.Empty, state, StringComparison.OrdinalIgnoreCase))
                        continue;

                    // --- name / q filter ---
                    var nameLower = name?.ToLowerInvariant();
                    var qLower = q?.ToLowerInvariant();

                    if (!string.IsNullOrWhiteSpace(nameLower))
                    {
                        if (!destDoc.Name.ToLowerInvariant().Contains(nameLower))
                            continue;
                    }

                    if (!string.IsNullOrWhiteSpace(qLower))
                    {
                        var combined = $"{destDoc.Name} {destDoc.Country} {destDoc.State} {trip.Description}".ToLowerInvariant();
                        if (!combined.Contains(qLower))
                            continue;
                    }

                    // --- capacity calculation ---
                    var maxBuddies = trip.MaxBuddies ?? 0;
                    var acceptedPersons = td.Buddies
                        .Where(b => string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase))
                        .Sum(b => b.PersonCount ?? 1);

                    var remaining = maxBuddies > 0
                        ? Math.Max(0, maxBuddies - acceptedPersons)
                        : 0;

                    if (partySize.HasValue && remaining < partySize.Value)
                        continue;

                    results.Add(new TripDestinationSearchResult
                    {
                        TripDestinationId = td.TripDestinationId,
                        TripId = trip.TripId,
                        DestinationId = td.DestinationId,
                        DestinationName = destDoc.Name,
                        Country = destDoc.Country,
                        State = destDoc.State ?? string.Empty,
                        DestinationStart = destStart,
                        DestinationEnd = destEnd,
                        MaxBuddies = maxBuddies,
                        AcceptedPersons = acceptedPersons,
                        RemainingCapacity = remaining
                    });
                }
            }

            return results;
        }

        public async Task<IEnumerable<Destination>> GetDestinationsAsync()
        {
            // TODO: implement 
            return await Task.FromResult<IEnumerable<Destination>>(new List<Destination>());
        }

        // ---------------------------------------------------------
        // Get trips that a user owns or is buddy on
        // ---------------------------------------------------------
        public async Task<IEnumerable<BuddyTripSummary>> GetBuddyTripsAsync(int userId)
        {
            var (trips, destMap) = await LoadTripsAndDestinationsAsync();
            var summaries = new List<BuddyTripSummary>();

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    if (!td.Buddies.Any(b =>
                            b.UserId == userId &&
                            string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase)
                        ))
                        continue;

                    summaries.Add(new BuddyTripSummary
                    {
                        TripId = trip.TripId,
                        TripDestinationId = td.TripDestinationId,
                        DestinationName = destDoc.Name,
                        TripDescription = trip.Description ?? string.Empty
                    });
                }
            }

            return summaries;
        }

        // ---------------------------------------------------------
        // Get trip destination info by id
        // ---------------------------------------------------------
        public async Task<TripDestinationInfo?> GetTripDestinationInfoAsync(int tripDestinationId)
        {
            // TODO implement
            return await Task.FromResult<TripDestinationInfo?>(null);
        }

        // ---------------------------------------------------------
        // Get full trip overview
        // ---------------------------------------------------------
        public async Task<TripOverview?> GetFullTripOverviewAsync(int tripId)
        {
            // TODO implement
            return await Task.FromResult<TripOverview?>(null);
        }

        // ---------------------------------------------------------
        // Get all trip overviews owned by a user
        // ---------------------------------------------------------
        public async Task<List<TripOverview>> GetOwnedTripOverviewsAsync(int userId)
        {
            // TODO implement
            return await Task.FromResult(new List<TripOverview>());
        }

        // ---------------------------------------------------------
        // Get owner of a specific trip destination
        // ---------------------------------------------------------
        public async Task<int?> GetTripOwnerAsync(int tripDestinationId)
        {
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == tripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            return trip?.OwnerId;
        }

        // ---------------------------------------------------------
        // Create a new trip with destinations
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> CreateTripWithDestinationsAsync(CreateTripWithDestinationsDto createTripWithDestinationsDto)
        {
            // TODO implement
            return await Task.FromResult((false, "Not implemented"));
        }

        // ---------------------------------------------------------
        // User leaves a trip destination (deactivate buddy)
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> LeaveTripDestinationAsync(
            int userId,
            int tripDestinationId,
            int triggeredBy,
            string departureReason)
        {
            // Load the trip containing this tripDestination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == tripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return (false, "Trip destination not found.");

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == tripDestinationId);
            if (td == null)
                return (false, "Trip destination not found.");

            var buddy = td.Buddies.FirstOrDefault(b => b.UserId == userId &&
                                                       string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase));
            if (buddy == null)
                return (false, "Buddy not found for this trip destination.");

            // Mark buddy as inactive and store reason
            buddy.IsActive = false;
            buddy.DepartureReason = departureReason;

            // (We leave RequestStatus as "accepted" – the relational model uses audits for 'left')

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == trip.TripId,
                trip);

            if (!replaceResult.IsAcknowledged || replaceResult.ModifiedCount == 0)
                return (false, "Failed to update trip in MongoDB.");

            return (true, null);
        }

        // ---------------------------------------------------------
        // Pending buddy requests for a trip owner
        // ---------------------------------------------------------
        public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
        {
            var trips = await LoadTripsAsync();

            // Only trips where this user is the owner
            var ownedTrips = trips.Where(t => t.OwnerId == userId).ToList();

            if (!ownedTrips.Any())
                return Enumerable.Empty<PendingBuddyRequest>();

            // Build lookups for destinations and users
            var destIds = ownedTrips
                .SelectMany(t => t.Destinations)
                .Select(td => td.DestinationId)
                .Distinct()
                .ToList();

            var destinationDocs = await _destinationsCollection
                .Find(d => destIds.Contains(d.DestinationId))
                .ToListAsync();

            var destMap = destinationDocs.ToDictionary(d => d.DestinationId, d => d);

            var userIds = ownedTrips
                .SelectMany(t => t.Destinations)
                .SelectMany(td => td.Buddies)
                .Where(b => string.Equals(b.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase))
                .Select(b => b.UserId)
                .Distinct()
                .ToList();

            var userDocs = await _usersCollection
                .Find(u => userIds.Contains(u.UserId))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.UserId, u => u);

            var result = new List<PendingBuddyRequest>();

            foreach (var trip in ownedTrips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    foreach (var b in td.Buddies.Where(b =>
                        string.Equals(b.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase)))
                    {
                        userMap.TryGetValue(b.UserId, out var uDoc);

                        result.Add(new PendingBuddyRequest
                        {
                            TripId = trip.TripId,
                            DestinationName = destDoc.Name,
                            BuddyId = b.BuddyId,
                            RequesterUserId = b.UserId,
                            RequesterName = uDoc?.Name ?? string.Empty,
                            BuddyNote = b.Note,
                            PersonCount = b.PersonCount ?? 1
                        });
                    }
                }
            }

            return result;
        }

        // ---------------------------------------------------------
        // User sends a new buddy request
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            // Find the trip containing this TripDestination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == buddyDto.TripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return (false, "Trip destination not found");

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == buddyDto.TripDestinationId);
            if (td == null)
                return (false, "Trip destination not found");

            // Check if user already has a request for this destination
            var existing = td.Buddies.FirstOrDefault(b => b.UserId == buddyDto.UserId);
            if (existing != null)
                return (false, "Buddy request already exists or user is already a member");

            var newBuddyId = await GetNextBuddyIdAsync();

            var buddy = new BuddyEmbedded
            {
                BuddyId = newBuddyId,
                UserId = buddyDto.UserId,
                PersonCount = buddyDto.PersonCount,
                Note = buddyDto.Note,
                IsActive = false,
                DepartureReason = null,
                RequestStatus = "pending"
            };

            td.Buddies.Add(buddy);

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == trip.TripId,
                trip);

            return (replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0, null);
        }

        // ---------------------------------------------------------
        // Owner accepts/rejects a buddy request
        // ---------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            var trips = await LoadTripsAsync();

            TripDocument? tripWithBuddy = null;
            TripDestinationEmbedded? tdWithBuddy = null;
            BuddyEmbedded? buddy = null;

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    var b = td.Buddies.FirstOrDefault(x => x.BuddyId == updateBuddyRequestDto.BuddyId);
                    if (b != null)
                    {
                        tripWithBuddy = trip;
                        tdWithBuddy = td;
                        buddy = b;
                        break;
                    }
                }
                if (buddy != null) break;
            }

            if (tripWithBuddy == null || tdWithBuddy == null || buddy == null)
                return (false, "Buddy request not found");

            var newStatus = updateBuddyRequestDto.NewStatus switch
            {
                BuddyRequestUpdateStatus.Accepted => "accepted",
                BuddyRequestUpdateStatus.Rejected => "rejected",
                _ => "pending"
            };

            buddy.RequestStatus = newStatus;

            if (newStatus == "accepted")
            {
                buddy.IsActive = true;
                buddy.DepartureReason = null;
            }
            else if (newStatus == "rejected")
            {
                buddy.IsActive = false;
            }

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == tripWithBuddy.TripId,
                tripWithBuddy);

            return (replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0, null);
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<TripAudit>> GetTripAuditsAsync()
        {
            // TODO Placeholder: Return an empty collection of TripAudit records
            return await Task.FromResult<IEnumerable<TripAudit>>(new List<TripAudit>());
        }
        public async Task<IEnumerable<BuddyAudit>> GetBuddyAuditsAsync()
        {
            // TODO Placeholder: Return an empty collection of BuddyAudit records
            return await Task.FromResult<IEnumerable<BuddyAudit>>(new List<BuddyAudit>());
        }
    }
}