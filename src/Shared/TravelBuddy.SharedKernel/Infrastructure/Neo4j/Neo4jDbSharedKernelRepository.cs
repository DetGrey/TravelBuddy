using Neo4j.Driver;
using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel;
// CLASS
public class Neo4jSharedKernelRepository : ISharedKernelRepository
{
    private readonly IDriver _driver;

    public Neo4jSharedKernelRepository(IDriver driver)
    {
        _driver = driver;
    }
    public async Task<IEnumerable<SystemEventLog>> GetSystemEventLogsAsync()
    {
        await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
        
        var result = await session.RunAsync(
            "MATCH (l:SystemEventLog) " +
            "RETURN l.eventId AS eventId, " +
            "       l.eventType AS eventType, " +
            "       l.affectedId AS affectedId, " +
            "       l.triggeredAt AS triggeredAt, " +
            "       l.details AS details " +
            "ORDER BY l.eventId DESC");

        var logs = new List<SystemEventLog>();
        
        await foreach (var record in result)
        {
            var log = new SystemEventLog
            {
                EventId = record["eventId"].As<int>(),
                EventType = record["eventType"].As<string>(),
                AffectedId = record["affectedId"] == null ? null : record["affectedId"].As<int?>(),
                TriggeredAt = ConvertNeo4jDateTime(record["triggeredAt"]),
                Details = record["details"] == null ? null : record["details"].As<string>()
            };
            logs.Add(log);
        }

        return logs;
    }

    private static DateTime? ConvertNeo4jDateTime(object? value)
    {
        if (value == null)
            return null;

        return value switch
        {
            DateTime dt => dt,
            Neo4j.Driver.LocalDateTime ldt => ldt.ToDateTime(),
            Neo4j.Driver.ZonedDateTime zdt => zdt.UtcDateTime,
            _ => null
        };
    }
}
