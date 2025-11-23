using MongoDB.Bson.Serialization.Attributes;

namespace TravelBuddy.Migrator.Models;

public class DestinationDocument
{
    [BsonId]
    public int DestinationId { get; set; }

    public string Name { get; set; } = null!;
    public string? State { get; set; }
    public string Country { get; set; } = null!;
    public decimal? Longitude { get; set; }
    public decimal? Latitude { get; set; }
}
