using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TravelBuddy.Migrator.Models;
using EFServerVersion = Microsoft.EntityFrameworkCore.ServerVersion;


// EF DbContexts + entities
using TravelBuddy.Users.Infrastructure;   
using TravelBuddy.Users.Models;

using TravelBuddy.Trips.Infrastructure;
using TravelBuddy.Trips.Models;

using TravelBuddy.Messaging.Infrastructure;
using TravelBuddy.Messaging.Models; 


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
    using var connection = usersDbContext.Database.GetDbConnection();
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

// ------------- MIGRATION START -------------
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


// Clear collections so you can re-run the migrator during development
await usersCollection.DeleteManyAsync(FilterDefinition<UserDocument>.Empty);
await destinationsCollection.DeleteManyAsync(FilterDefinition<DestinationDocument>.Empty);
await tripsCollection.DeleteManyAsync(FilterDefinition<TripDocument>.Empty);
await conversationsCollection.DeleteManyAsync(FilterDefinition<ConversationDocument>.Empty);
await messagesCollection.DeleteManyAsync(FilterDefinition<MessageDocument>.Empty);

// ===== 1) USERS =====
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
    Id = MongoDB.Bson.ObjectId.GenerateNewId(),

    LegacyUserId = u.UserId,          
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

Console.WriteLine("Migration complete.");