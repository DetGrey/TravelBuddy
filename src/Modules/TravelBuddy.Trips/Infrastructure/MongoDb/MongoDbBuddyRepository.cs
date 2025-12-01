using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.Models;

namespace TravelBuddy.Trips
{
    // ---------- Mongo document shapes specifically for this repository ----------

    [BsonIgnoreExtraElements]
    internal class BuddyEmbeddedForBuddy
    {
        public int BuddyId { get; set; }
        public int UserId { get; set; }
        public int? PersonCount { get; set; }
        public string? Note { get; set; }
        public bool? IsActive { get; set; }
        public string? DepartureReason { get; set; }
        public string RequestStatus { get; set; } = string.Empty;
    }

    [BsonIgnoreExtraElements]
    internal class TripDestinationEmbeddedForBuddy
    {
        public int TripDestinationId { get; set; }
        public int DestinationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int SequenceNumber { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }
        public List<BuddyEmbeddedForBuddy> Buddies { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class TripDocForBuddy
    {
        public int TripId { get; set; }
        public int? OwnerId { get; set; }
        public int? MaxBuddies { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }

        public List<TripDestinationEmbeddedForBuddy> Destinations { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class DestinationDocForBuddy
    {
        [BsonId]
        public int DestinationId { get; set; }

        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class UserDocForBuddy
    {
        [BsonElement("LegacyUserId")]
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    // ---------- Repository implementation ----------

    public class MongoDbBuddyRepository : IBuddyRepository
    {
        private readonly IMongoCollection<TripDocForBuddy> _tripsCollection;
        private readonly IMongoCollection<DestinationDocForBuddy> _destinationsCollection;
        private readonly IMongoCollection<UserDocForBuddy> _usersCollection;

        public MongoDbBuddyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _tripsCollection = database.GetCollection<TripDocForBuddy>("trips");
            _destinationsCollection = database.GetCollection<DestinationDocForBuddy>("destinations");
            _usersCollection = database.GetCollection<UserDocForBuddy>("users");
        }

        private async Task<List<TripDocForBuddy>> LoadTripsAsync()
        {
            return await _tripsCollection
                .Find(FilterDefinition<TripDocForBuddy>.Empty)
                .ToListAsync();
        }

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
        // Pending buddy requests for an OWNER user
        // TODO Doesn't work, it won't return a reponse
        // ---------------------------------------------------------
        public async Task<IEnumerable<PendingBuddyRequest>> GetPendingBuddyRequestsAsync(int userId)
        {
            
            var trips = await LoadTripsAsync();

            // Only trips where this user is the OWNER
            var ownedTrips = trips.Where(t => t.OwnerId == userId).ToList();
            if (!ownedTrips.Any())
                return Enumerable.Empty<PendingBuddyRequest>();

            // Collect destination IDs and user IDs involved in pending requests
            var destIds = ownedTrips
                .SelectMany(t => t.Destinations)
                .Select(td => td.DestinationId)
                .Distinct()
                .ToList();

            var destinationDocs = await _destinationsCollection
                .Find(d => destIds.Contains(d.DestinationId))
                .ToListAsync();

            var destMap = destinationDocs
                .GroupBy(d => d.DestinationId)
                .ToDictionary(g => g.Key, g => g.First());

            var pendingBuddies = ownedTrips
                .SelectMany(t => t.Destinations, (t, td) => new { Trip = t, TripDest = td })
                .SelectMany(
                    x => x.TripDest.Buddies,
                    (x, b) => new { x.Trip, x.TripDest, Buddy = b }
                )
                .Where(x => string.Equals(x.Buddy.RequestStatus, "pending", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var buddyUserIds = pendingBuddies
                .Select(x => x.Buddy.UserId)
                .Distinct()
                .ToList();

            var userDocs = await _usersCollection
                .Find(u => buddyUserIds.Contains(u.UserId))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.UserId, u => u);

            var result = new List<PendingBuddyRequest>();

            foreach (var item in pendingBuddies)
            {
                if (!destMap.TryGetValue(item.TripDest.DestinationId, out var destDoc))
                    continue;

                userMap.TryGetValue(item.Buddy.UserId, out var uDoc);

                result.Add(new PendingBuddyRequest
                {
                    TripId = item.Trip.TripId,
                    DestinationName = destDoc.Name,
                    BuddyId = item.Buddy.BuddyId,
                    UserId = item.Buddy.UserId,
                    BuddyName = uDoc?.Name ?? string.Empty,
                    BuddyNote = item.Buddy.Note,
                    PersonCount = item.Buddy.PersonCount ?? 1
                });
            }

            return result;
            
        }

        // ---------------------------------------------------------
        // User sends a new buddy request (as buddy)
        // ---------------------------------------------------------
        public async Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            // Find the trip containing this TripDestination
            var filter = Builders<TripDocForBuddy>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == buddyDto.TripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return false;

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == buddyDto.TripDestinationId);
            if (td == null)
                return false;

            // Check if user already has a buddy entry here
            var existing = td.Buddies.FirstOrDefault(b => b.UserId == buddyDto.UserId);
            if (existing != null)
                return false;

            var newBuddyId = await GetNextBuddyIdAsync();

            var buddy = new BuddyEmbeddedForBuddy
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

            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }

        // ---------------------------------------------------------
        // Owner accepts/rejects a buddy request
        // ---------------------------------------------------------
        public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            var trips = await LoadTripsAsync();

            TripDocForBuddy? foundTrip = null;
            TripDestinationEmbeddedForBuddy? foundDest = null;
            BuddyEmbeddedForBuddy? foundBuddy = null;

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    var b = td.Buddies.FirstOrDefault(x => x.BuddyId == updateBuddyRequestDto.BuddyId);
                    if (b != null)
                    {
                        foundTrip = trip;
                        foundDest = td;
                        foundBuddy = b;
                        break;
                    }
                }
                if (foundBuddy != null)
                    break;
            }

            if (foundTrip == null || foundDest == null || foundBuddy == null)
                return false;

            // Ensure caller is the owner
            if (foundTrip.OwnerId != updateBuddyRequestDto.UserId)
                return false;

            var newStatus = updateBuddyRequestDto.NewStatus switch
            {
                BuddyRequestUpdateStatus.Accepted => "accepted",
                BuddyRequestUpdateStatus.Rejected => "rejected",
                _ => "pending"
            };

            foundBuddy.RequestStatus = newStatus;

            if (newStatus == "accepted")
            {
                foundBuddy.IsActive = true;
                foundBuddy.DepartureReason = null;
            }
            else if (newStatus == "rejected")
            {
                foundBuddy.IsActive = false;
            }

            var replaceResult = await _tripsCollection.ReplaceOneAsync(
                t => t.TripId == foundTrip.TripId,
                foundTrip);

            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }
    }
}