using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelBuddy.Trips.Models;
using TravelBuddy.Trips.Infrastructure;

namespace TravelBuddy.Trips
{
    // ---------- Mongo document shapes (match migrator) ----------

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
        public DateTime StartDate { get; set; }   // stored as DateTime in Mongo
        public DateTime EndDate { get; set; }
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public bool? IsArchived { get; set; }

        public List<TripDestinationEmbedded> Destinations { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class DestinationDocument
    {
        [BsonId]
        public int DestinationId { get; set; }

        public string Name { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
    }

    // ---------- Repository implementation ----------

    public class MongoDbTripDestinationRepository : ITripDestinationRepository
    {
        private readonly IMongoCollection<TripDocument> _tripsCollection;
        private readonly IMongoCollection<DestinationDocument> _destinationsCollection;

        public MongoDbTripDestinationRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _tripsCollection = database.GetCollection<TripDocument>("trips");
            _destinationsCollection = database.GetCollection<DestinationDocument>("destinations");
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

                    var destStart = DateOnly.FromDateTime(td.StartDate);
                    var destEnd = DateOnly.FromDateTime(td.EndDate);

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

        // ---------------------------------------------------------
        // Get trips that a user owns or is buddy on
        // ---------------------------------------------------------
        public async Task<IEnumerable<UserTripSummary>> GetUserTripsAsync(int userId)
        {
            var (trips, destMap) = await LoadTripsAndDestinationsAsync();
            var summaries = new List<UserTripSummary>();

            foreach (var trip in trips)
            {
                foreach (var td in trip.Destinations)
                {
                    if (!destMap.TryGetValue(td.DestinationId, out var destDoc))
                        continue;

                    string? role = null;

                    if (trip.OwnerId == userId)
                    {
                        role = "owner";
                    }
                    else if (td.Buddies.Any(b =>
                        b.UserId == userId &&
                        string.Equals(b.RequestStatus, "accepted", StringComparison.OrdinalIgnoreCase)))
                    {
                        role = "buddy";
                    }

                    if (role == null)
                        continue;

                    summaries.Add(new UserTripSummary
                    {
                        TripId = trip.TripId,
                        TripDestinationId = td.TripDestinationId,
                        DestinationName = destDoc.Name,
                        TripDescription = trip.Description ?? string.Empty,
                        Role = role
                    });
                }
            }

            return summaries;
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
    }
}