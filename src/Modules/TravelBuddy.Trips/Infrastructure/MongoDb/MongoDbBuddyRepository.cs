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
    // NOTE: TripDocument / TripDestinationEmbedded / BuddyEmbedded / DestinationDocument
    // are defined in MongoDbTripDestinationRepository.cs in the same namespace and
    // are reused here.

    [BsonIgnoreExtraElements]
    internal class UserDocument
    {
        [BsonElement("LegacyUserId")]
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
    }

    public class MongoDbBuddyRepository : IBuddyRepository
    {
        private readonly IMongoCollection<TripDocument> _tripsCollection;
        private readonly IMongoCollection<DestinationDocument> _destinationsCollection;
        private readonly IMongoCollection<UserDocument> _usersCollection;

        public MongoDbBuddyRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _tripsCollection = database.GetCollection<TripDocument>("trips");
            _destinationsCollection = database.GetCollection<DestinationDocument>("destinations");
            _usersCollection = database.GetCollection<UserDocument>("users");
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
                            UserId = b.UserId,
                            BuddyName = uDoc?.Name ?? string.Empty,
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
        public async Task<bool> InsertBuddyRequestAsync(BuddyDto buddyDto)
        {
            // Find the trip containing this TripDestination
            var filter = Builders<TripDocument>.Filter
                .ElemMatch(t => t.Destinations, td => td.TripDestinationId == buddyDto.TripDestinationId);

            var trip = await _tripsCollection.Find(filter).FirstOrDefaultAsync();
            if (trip == null)
                return false;

            var td = trip.Destinations.FirstOrDefault(d => d.TripDestinationId == buddyDto.TripDestinationId);
            if (td == null)
                return false;

            // Check if user already has a request for this destination
            var existing = td.Buddies.FirstOrDefault(b => b.UserId == buddyDto.UserId);
            if (existing != null)
                return false; // already requested / member

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

            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }

        // ---------------------------------------------------------
        // Owner accepts/rejects a buddy request
        // ---------------------------------------------------------
        public async Task<bool> UpdateBuddyRequestAsync(UpdateBuddyRequestDto updateBuddyRequestDto)
        {
            // Load all trips (small dataset assumption)
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
                return false;

            // Optional: ensure the caller is the owner of this trip
            if (tripWithBuddy.OwnerId != updateBuddyRequestDto.UserId)
                return false;

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

            return replaceResult.IsAcknowledged && replaceResult.ModifiedCount > 0;
        }
    }
}
