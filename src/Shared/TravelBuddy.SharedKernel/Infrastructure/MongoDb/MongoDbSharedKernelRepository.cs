using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel;

// MongoDB Document Model for SystemEventLog
[BsonIgnoreExtraElements]
public class SystemEventLogDocument
{
    [BsonId]
    public int EventId { get; set; }

    public string EventType { get; set; } = null!;

    public int? AffectedId { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Details { get; set; }
}

// CLASS
public class MongoDbSharedKernelRepository : ISharedKernelRepository
{
    private readonly IMongoCollection<SystemEventLogDocument> _systemEventLogCollection;

    public MongoDbSharedKernelRepository(IMongoClient client)
    {
        // Same DB name as your migrator / other Mongo repos
        var database = client.GetDatabase("travel_buddy_mongo");
        _systemEventLogCollection = database.GetCollection<SystemEventLogDocument>("system_event_logs");
    }
    public async Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync()
    {
        var docs = await _systemEventLogCollection
            .Find(FilterDefinition<SystemEventLogDocument>.Empty)
            .ToListAsync();

        return docs.Select(doc => new SystemEventLog
        {
            EventId = doc.EventId,
            EventType = doc.EventType,
            AffectedId = doc.AffectedId,
            Timestamp = doc.Timestamp,
            Details = doc.Details
        }).ToList();
    }
}
