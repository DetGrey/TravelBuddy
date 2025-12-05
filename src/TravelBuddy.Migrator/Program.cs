using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Neo4j.Driver;
using TravelBuddy.Migrator.Models;
using EFServerVersion = Microsoft.EntityFrameworkCore.ServerVersion;


// EF DbContexts + entities
using TravelBuddy.Users.Infrastructure;
using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Messaging.Infrastructure;


// Load .env from solution root
Env.Load();

// ------------- CONFIG -------------
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

// ---- Read variable *names* from appsettings.json ----
var mysqlSection = configuration.GetSection("MySql");
// These are names like "MYSQL_HOST", "MYSQL_PORT", ...
var hostVar = mysqlSection["Host"] ?? throw new InvalidOperationException("MySql:Host not set in appsettings.json");
var portVar = mysqlSection["Port"] ?? "MYSQL_PORT";
var userVar = mysqlSection["User"] ?? throw new InvalidOperationException("MySql:User not set in appsettings.json");
var passVar = mysqlSection["Password"] ?? throw new InvalidOperationException("MySql:Password not set in appsettings.json");
var dbVar   = mysqlSection["Database"] ?? throw new InvalidOperationException("MySql:Database not set in appsettings.json");

// ---- Look up actual values in .env using those names ----
var mysqlHost = Env.GetString(hostVar) ?? throw new InvalidOperationException($"Env var {hostVar} not set");
var mysqlPort = Env.GetString(portVar) ?? "3306";
var mysqlUser = Env.GetString(userVar) ?? throw new InvalidOperationException($"Env var {userVar} not set");
var mysqlPass = Env.GetString(passVar) ?? throw new InvalidOperationException($"Env var {passVar} not set");
var mysqlDb   = Env.GetString(dbVar)   ?? throw new InvalidOperationException($"Env var {dbVar} not set");

// Final MySQL connection string
var mySqlConnectionString =
    $"Server={mysqlHost};Port={mysqlPort};Database={mysqlDb};User={mysqlUser};Password={mysqlPass};";

// ---- Mongo: same pattern ----
var mongoSection = configuration.GetSection("Mongo");
var mongoConnVar = mongoSection["Connection"] ?? throw new InvalidOperationException("Mongo:Connection not set in appsettings.json");
var mongoDbVar   = mongoSection["Database"]   ?? "MONGO_DATABASE";

var mongoConnectionString = Env.GetString(mongoConnVar)
    ?? throw new InvalidOperationException($"Env var {mongoConnVar} not set");

var mongoDatabaseName = Env.GetString(mongoDbVar) 
    ?? "travel_buddy_mongo";

// ---- Neo4j settings via appsettings + .env ----
var neo4jSection = configuration.GetSection("Neo4j");

var neo4jUriVar  = neo4jSection["Uri"]      ?? "NEO4J_URI";
var neo4jUserVar = neo4jSection["User"]     ?? "NEO4J_USER";
var neo4jPassVar = neo4jSection["Password"] ?? "NEO4J_PASSWORD";

var neo4jUri  = Env.GetString(neo4jUriVar) 
    ?? throw new InvalidOperationException($"Env var {neo4jUriVar} not set");
var neo4jUser = Env.GetString(neo4jUserVar) 
    ?? throw new InvalidOperationException($"Env var {neo4jUserVar} not set");
var neo4jPass = Env.GetString(neo4jPassVar) 
    ?? throw new InvalidOperationException($"Env var {neo4jPassVar} not set");

Console.WriteLine("Configuration:");
Console.WriteLine($"MySQL connection string: {mySqlConnectionString}");
Console.WriteLine($"MongoDB connection string: {mongoConnectionString}");
Console.WriteLine($"Neo4j URI being used: {neo4jUri}");

// Create Neo4j driver
IDriver neo4jDriver = GraphDatabase.Driver(neo4jUri, AuthTokens.Basic(neo4jUser, neo4jPass));
await using var neo4j = neo4jDriver;

// ------------- EF CORE DB CONTEXTS -------------
var usersOptions = new DbContextOptionsBuilder<UsersDbContext>()
    .UseMySql(mySqlConnectionString, EFServerVersion.AutoDetect(mySqlConnectionString))
    .Options;


var tripsOptions = new DbContextOptionsBuilder<TripsDbContext>()
    .UseMySql(mySqlConnectionString, EFServerVersion.AutoDetect(mySqlConnectionString))
    .Options;


var messagingOptions = new DbContextOptionsBuilder<MessagingDbContext>()
    .UseMySql(mySqlConnectionString, EFServerVersion.AutoDetect(mySqlConnectionString))
    .Options;

using var usersDbContext = new UsersDbContext(usersOptions);
using var tripsDbContext = new TripsDbContext(tripsOptions);
using var messagingDbContext = new MessagingDbContext(messagingOptions);

// ---- Test MySQL connection ----
try
{
    Console.WriteLine("Testing MySQL connection...");
    var connection = usersDbContext.Database.GetDbConnection();
    await connection.OpenAsync();
    Console.WriteLine("MySQL connection OK!");
    await connection.CloseAsync();
}
catch (Exception ex)
{
    Console.WriteLine("MySQL connection FAILED:");
    Console.WriteLine(ex.Message);
    return;
}

// ---- Set up MongoDB ----
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

var usersCollection = mongoDatabase.GetCollection<UserDocument>("users");
var destinationsCollection  = mongoDatabase.GetCollection<DestinationDocument>("destinations");
var tripsCollection         = mongoDatabase.GetCollection<TripDocument>("trips");
var conversationsCollection = mongoDatabase.GetCollection<ConversationDocument>("conversations");
var messagesCollection      = mongoDatabase.GetCollection<MessageDocument>("messages");

// ------------- MONGODB MIGRATION START -------------
Console.WriteLine("Starting user migration...");

// ---- Test Mongo Connection ----
try
{
    var dbNames = await mongoClient.ListDatabaseNamesAsync();
    Console.WriteLine("MongoDB connection OK. Databases:");
    await foreach (var name in dbNames.ToAsyncEnumerable())
    {
        Console.WriteLine($" - {name}");
    }
}
catch (Exception ex)
{
    Console.WriteLine("MongoDB connection FAILED:");
    Console.WriteLine(ex.Message);
    return;
}

// Test Neo4j connection
try
{
    Console.WriteLine("Testing Neo4j connection...");
    await using var testSession = neo4j.AsyncSession();
    var cursor = await testSession.RunAsync("RETURN 'Neo4j OK' AS msg");
    var rec = await cursor.SingleAsync();
    Console.WriteLine($"{rec["msg"].As<string>()}");
}
catch (Exception ex)
{
    Console.WriteLine("Neo4j connection FAILED:");
    Console.WriteLine(ex.Message);
    return;
}

// Clear collections so you can re-run the migrator during development
await usersCollection.DeleteManyAsync(FilterDefinition<UserDocument>.Empty);
await destinationsCollection.DeleteManyAsync(FilterDefinition<DestinationDocument>.Empty);
await tripsCollection.DeleteManyAsync(FilterDefinition<TripDocument>.Empty);
await conversationsCollection.DeleteManyAsync(FilterDefinition<ConversationDocument>.Empty);
await messagesCollection.DeleteManyAsync(FilterDefinition<MessageDocument>.Empty);

// ===== 1) MongoDB: USERS =====
var users = await usersDbContext.Users
    .AsNoTracking()
    .ToListAsync();

if (users.Count == 0)
{
    Console.WriteLine("No users found in MySQL");
    return;
}


// Map from your EF User entity to Mongo UserDocument
var userDocs = users.Select(u => new UserDocument
{
    UserId       = u.UserId,          
    Name         = u.Name,        
    Email        = u.Email,
    PasswordHash = u.PasswordHash,
    Birthdate    = u.Birthdate,
    IsDeleted    = u.IsDeleted,
    Role         = u.Role

}).ToList();

await usersCollection.InsertManyAsync(userDocs);

Console.WriteLine($"Migrated {userDocs.Count} users to MongoDB.");


// ===== 2) DESTINATIONS (flat collection) =====
Console.WriteLine("Migrating destinations...");

// TODO: DbSet name probably "Destinations"
var destinations = await tripsDbContext.Destinations
    .AsNoTracking()
    .ToListAsync();

var destinationDocs = destinations.Select(d => new DestinationDocument
{
    DestinationId = d.DestinationId,
    Name          = d.Name,
    State         = d.State,
    Country       = d.Country,
    Longitude     = d.Longitude,
    Latitude      = d.Latitude
}).ToList();

if (destinationDocs.Count > 0)
{
    await destinationsCollection.InsertManyAsync(destinationDocs);
}
Console.WriteLine($"Destinations migrated: {destinationDocs.Count}");

// ===== 3) TRIPS with embedded TRIP_DESTINATIONS + BUDDIES =====
Console.WriteLine("Migrating trips with embedded trip_destinations and buddies...");


var trips = await tripsDbContext.Trips
    .AsNoTracking()
    .ToListAsync();

var tripDestinations = await tripsDbContext.TripDestinations
    .AsNoTracking()
    .ToListAsync();

var buddies = await tripsDbContext.Buddies
    .AsNoTracking()
    .ToListAsync();

// Group trip_destinations by TripId
var tripDestByTripId = tripDestinations
    .GroupBy(td => td.TripId)
    .ToDictionary(g => g.Key, g => g.ToList());

// Group buddies by TripDestinationId
var buddiesByTripDestId = buddies
    .GroupBy(b => b.TripDestinationId)
    .ToDictionary(g => g.Key, g => g.ToList());

var tripDocs = trips.Select(t =>
{
    var tripDoc = new TripDocument
    {
        TripId      = t.TripId,
        OwnerId     = t.OwnerId,
        TripName    = t.TripName,
        MaxBuddies  = t.MaxBuddies,
        StartDate   = t.StartDate,
        EndDate     = t.EndDate,
        Description = t.Description,
        IsArchived  = t.IsArchived,
        Destinations = new List<TripDestinationEmbedded>()
    };

    if (tripDestByTripId.TryGetValue(t.TripId, out var tdList))
    {
        foreach (var td in tdList)
        {
            var destEmbedded = new TripDestinationEmbedded
            {
                TripDestinationId = td.TripDestinationId,
                DestinationId     = td.DestinationId,
                StartDate         = td.StartDate,
                EndDate           = td.EndDate,
                SequenceNumber    = td.SequenceNumber,
                Description       = td.Description,
                IsArchived        = td.IsArchived,
                Buddies           = new List<BuddyEmbedded>()
            };

            if (buddiesByTripDestId.TryGetValue(td.TripDestinationId, out var buddyList))
            {
                foreach (var b in buddyList)
                {
                    destEmbedded.Buddies.Add(new BuddyEmbedded
                    {
                        BuddyId         = b.BuddyId,
                        UserId          = b.UserId,
                        PersonCount     = b.PersonCount,
                        Note            = b.Note,
                        IsActive        = b.IsActive,
                        DepartureReason = b.DepartureReason,
                        RequestStatus   = b.RequestStatus
                    });
                }
            }

            tripDoc.Destinations.Add(destEmbedded);
        }
    }

    return tripDoc;
}).ToList();

if (tripDocs.Count > 0)
{
    await tripsCollection.InsertManyAsync(tripDocs);
}
Console.WriteLine($"Trips migrated (with embedded destinations & buddies): {tripDocs.Count}");

// ===== 4) CONVERSATIONS with embedded PARTICIPANTS =====
Console.WriteLine("Migrating conversations with embedded participants...");

// TODO: adjust DbSet names & property names to your Messaging module
var conversations = await messagingDbContext.Conversations
    .AsNoTracking()
    .ToListAsync();

var convParticipants = await messagingDbContext.ConversationParticipants
    .AsNoTracking()
    .ToListAsync();

// Group participants by ConversationId
var participantsByConversationId = convParticipants
    .GroupBy(cp => cp.ConversationId)
    .ToDictionary(g => g.Key, g => g.ToList());

var conversationDocs = conversations.Select(c =>
{
    var convDoc = new ConversationDocument
    {
        ConversationId    = c.ConversationId,
        TripDestinationId = c.TripDestinationId,
        IsGroup           = c.IsGroup,
        CreatedAt         = c.CreatedAt,
        IsArchived        = c.IsArchived,
        Participants      = new List<ConversationParticipantEmbedded>()
    };

    if (participantsByConversationId.TryGetValue(c.ConversationId, out var partList))
    {
        foreach (var p in partList)
        {
            convDoc.Participants.Add(new ConversationParticipantEmbedded
            {
                UserId   = p.UserId,
                JoinedAt = p.JoinedAt
            });
        }
    }

    return convDoc;
}).ToList();

if (conversationDocs.Count > 0)
{
    await conversationsCollection.InsertManyAsync(conversationDocs);
}
Console.WriteLine($"Conversations migrated (with embedded participants): {conversationDocs.Count}");

// ===== 5) MESSAGES (flat collection) =====
Console.WriteLine("Migrating messages...");

var messages = await messagingDbContext.Messages
    .AsNoTracking()
    .ToListAsync();

var messageDocs = messages.Select(m => new MessageDocument
{
    MessageId      = m.MessageId,
    ConversationId = m.ConversationId,
    SenderId       = m.SenderId ?? throw new InvalidOperationException($"Message {m.MessageId} has no SenderId"),
    Content        = m.Content,
    SentAt         = m.SentAt
}).ToList();

if (messageDocs.Count > 0)
{
    await messagesCollection.InsertManyAsync(messageDocs);
}
Console.WriteLine($"Messages migrated: {messageDocs.Count}");


// ===== Neo4j: MIGRATION =====
Console.WriteLine("Starting Neo4j migration...");

// ===== Neo4j: USERS =====
Console.WriteLine("Migrating users to Neo4j...");

await using (var userSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var u in users)
    {
        var parameters = new
        {
            userId    = u.UserId,
            name      = u.Name,
            email     = u.Email,
            passwordHash  = u.PasswordHash,
            birthdate = u.Birthdate.ToString("yyyy-MM-dd"),
            isDeleted = u.IsDeleted,
            role      = u.Role
        };

        await userSession.RunAsync(@"
            MERGE (u:User { userId: $userId })
            SET u.name         = $name,
                u.email        = $email,
                u.passwordHash = $passwordHash,
                u.birthdate    = $birthdate,
                u.isDeleted    = $isDeleted,
                u.role         = $role
        ", parameters);
    }
}

Console.WriteLine($"Neo4j: migrated {users.Count} users.");

// ===== Neo4j: DESTINATIONS =====
Console.WriteLine("Migrating destinations to Neo4j...");

await using (var destSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var d in destinations)
    {
        var parameters = new
        {
            destinationId = d.DestinationId,
            name          = d.Name,
            state         = d.State,
            country       = d.Country,
            longitude     = d.Longitude,
            latitude      = d.Latitude
        };

        await destSession.RunAsync(@"
            MERGE (d:Destination { destinationId: $destinationId })
            SET d.name      = $name,
                d.state     = $state,
                d.country   = $country,
                d.longitude = $longitude,
                d.latitude  = $latitude
        ", parameters);
    }
}

Console.WriteLine($"Neo4j: migrated {destinations.Count} destinations.");

// ===== Neo4j: TRIPS + OWNS + HAS_STOP =====
Console.WriteLine("Migrating trips and relationships to Neo4j...");

await using (var tripSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var t in trips)
    {
        var tripParams = new
        {
            tripId      = t.TripId,
            ownerId     = t.OwnerId ?? 0, // if OwnerId is int? in EF
            tripName    = t.TripName,
            maxBuddies  = t.MaxBuddies,
            startDate   = t.StartDate.ToString("yyyy-MM-dd"),
            endDate     = t.EndDate.ToString("yyyy-MM-dd"),
            description = t.Description,
            isArchived  = t.IsArchived
        };

        // Trip node + OWNS relationship
        await tripSession.RunAsync(@"
            MERGE (t:Trip { tripId: $tripId })
            SET t.tripName    = $tripName,
                t.maxBuddies  = $maxBuddies,
                t.startDate   = $startDate,
                t.endDate     = $endDate,
                t.description = $description,
                t.isArchived  = $isArchived
            WITH t, $ownerId AS ownerId
            MATCH (u:User { userId: ownerId })
            MERGE (u)-[:OWNS]->(t)
        ", tripParams);

        // HAS_STOP relationships for this trip
        var tdForTrip = tripDestinations.Where(td => td.TripId == t.TripId).ToList();

        foreach (var td in tdForTrip)
        {
            var tdParams = new
            {
                tripId            = t.TripId,
                tripDestinationId = td.TripDestinationId,
                destinationId     = td.DestinationId,
                startDate         = td.StartDate.ToString("yyyy-MM-dd"),
                endDate           = td.EndDate.ToString("yyyy-MM-dd"),
                sequenceNumber    = td.SequenceNumber,
                description       = td.Description,
                isArchived        = td.IsArchived
            };

            await tripSession.RunAsync(@"
                MATCH (t:Trip { tripId: $tripId })
                MATCH (d:Destination { destinationId: $destinationId })
                MERGE (t)-[r:HAS_STOP { tripDestinationId: $tripDestinationId }]->(d)
                SET r.startDate      = $startDate,
                    r.endDate        = $endDate,
                    r.sequenceNumber = $sequenceNumber,
                    r.description    = $description,
                    r.isArchived     = $isArchived
            ", tdParams);
        }
    }
}

Console.WriteLine($"Neo4j: migrated {trips.Count} trips and HAS_STOP relationships.");

// ===== Neo4j: BUDDIES =====
Console.WriteLine("Migrating buddies to Neo4j...");

await using (var buddySession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var b in buddies)
    {
        // Find trip for this buddy via TripDestination
        var td = tripDestinations.FirstOrDefault(td => td.TripDestinationId == b.TripDestinationId);
        if (td == null) continue;

        var buddyParams = new
        {
            userId            = b.UserId,
            tripId            = td.TripId,
            tripDestinationId = b.TripDestinationId,
            buddyId           = b.BuddyId,         
            personCount       = b.PersonCount,
            note              = b.Note,
            isActive          = b.IsActive,
            departureReason   = b.DepartureReason,
            requestStatus     = b.RequestStatus
        };

        await buddySession.RunAsync(@"
            MATCH (u:User { userId: $userId })
            MATCH (t:Trip { tripId: $tripId })
            MERGE (u)-[r:BUDDY_ON { tripDestinationId: $tripDestinationId }]->(t)
            SET  r.buddyId         = $buddyId,       
                 r.personCount     = $personCount,
                 r.note            = $note,
                 r.isActive        = $isActive,
                 r.departureReason = $departureReason,
                 r.requestStatus   = $requestStatus
        ", buddyParams);
    }
}

Console.WriteLine($"Neo4j: migrated {buddies.Count} buddies.");

// ===== Neo4j: CONVERSATIONS =====
Console.WriteLine("Migrating conversations to Neo4j...");

await using (var convSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var c in conversations)
    {
        var convParams = new
        {
            conversationId    = c.ConversationId,
            tripDestinationId = c.TripDestinationId,
            isGroup           = c.IsGroup,
            // Use ISO 8601 and handle nulls
            createdAt         = c.CreatedAt?.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture),
            isArchived        = c.IsArchived
        };

        await convSession.RunAsync(@"
            MERGE (c:Conversation { conversationId: $conversationId })
            SET c.tripDestinationId = $tripDestinationId,
                c.isGroup           = $isGroup,
                c.isArchived        = $isArchived

            // carry forward values for conditional createdAt
            WITH c, $createdAt AS createdAtIso, $tripDestinationId AS tdId
            UNWIND CASE WHEN createdAtIso IS NULL THEN [] ELSE [createdAtIso] END AS createdAtVal
            SET c.createdAt = datetime(createdAtVal)

            // carry forward again before next UNWIND
            WITH c, tdId
            UNWIND CASE WHEN tdId IS NULL THEN [] ELSE [tdId] END AS destId
            MATCH (td:TripDestination { tripDestinationId: destId })
            MERGE (c)-[:RELATES_TO]->(td)
        ", convParams);
    }
}
Console.WriteLine($"Neo4j: migrated {conversations.Count} conversations.");


// ===== Neo4j: CONVERSATION PARTICIPANTS =====
Console.WriteLine("Migrating conversation participants to Neo4j...");

await using (var cpSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var cp in convParticipants)
    {
        var cpParams = new
        {
            conversationId = cp.ConversationId,
            userId         = cp.UserId,
            joinedAtIso    = cp.JoinedAt?.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)
        };

        await cpSession.RunAsync(@"
            MATCH (u:User { userId: $userId })
            MATCH (c:Conversation { conversationId: $conversationId })
            MERGE (u)-[r:PARTICIPATES_IN]->(c)
            SET r.joinedAt = datetime($joinedAtIso)
        ", cpParams);
    }
}
Console.WriteLine($"Neo4j: migrated {convParticipants.Count} conversation participants.");


// ===== Neo4j: MESSAGES =====
Console.WriteLine("Migrating messages to Neo4j...");

await using (var msgSession = neo4j.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write)))
{
    foreach (var m in messages)
    {
        var msgParams = new
        {
            messageId      = m.MessageId,
            conversationId = m.ConversationId,
            senderId       = m.SenderId ?? throw new InvalidOperationException($"Message {m.MessageId} missing SenderId"),
            content        = m.Content,
            sentAtIso      = m.SentAt?.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)
        };

        await msgSession.RunAsync(@"
            MERGE (m:Message { messageId: $messageId })
            SET m.content = $content,
                m.sentAt  = datetime($sentAtIso)
            WITH m
            MATCH (u:User { userId: $senderId })
            MATCH (c:Conversation { conversationId: $conversationId })
            MERGE (u)-[:SENT]->(m)
            MERGE (c)-[:HAS_MESSAGE]->(m)
        ", msgParams);

    }
}
Console.WriteLine($"Neo4j: migrated {messages.Count} messages.");

Console.WriteLine("Migration complete.");