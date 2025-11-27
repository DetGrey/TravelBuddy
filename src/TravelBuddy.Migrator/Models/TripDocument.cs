using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class TripDocument
{
    [BsonId]
    public int TripId { get; set; }      // same as MySQL PK

    public int? OwnerId { get; set; }
    public int? MaxBuddies { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public string? Description { get; set; }
    public bool? IsArchived { get; set; }

    // Embedded trip_destinations
    public List<TripDestinationEmbedded> Destinations { get; set; } = new();
}

public class TripDestinationEmbedded
{
    public int TripDestinationId { get; set; }
    public int DestinationId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public int SequenceNumber { get; set; }
    public string? Description { get; set; }
    public bool? IsArchived { get; set; }

    // Embedded buddies for this leg of the trip
    public List<BuddyEmbedded> Buddies { get; set; } = new();
}

public class BuddyEmbedded
{
    public int BuddyId { get; set; }
    public int UserId { get; set; }
    public int? PersonCount { get; set; }
    public string? Note { get; set; }
    public bool? IsActive { get; set; }
    public string? DepartureReason { get; set; }
    public string RequestStatus { get; set; } = null!; // 'pending', 'accepted', 'rejected'
}

