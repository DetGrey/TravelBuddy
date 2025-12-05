using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging
{
    // -----------------------------
    // MongoDB document definitions
    // -----------------------------

    [BsonIgnoreExtraElements]
    internal class UserDocument
    {
        // IMPORTANT: map to Mongo _id (int)
        [BsonId]
        public int UserId { get; set; }

        // These property names match your Mongo fields:
        // Name, Email, PasswordHash, Birthdate, IsDeleted, Role
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? Birthdate { get; set; }
        public bool IsDeleted { get; set; }
        public string Role { get; set; } = null!;
    }

    [BsonIgnoreExtraElements]
    internal class ConversationParticipantEmbedded
    {
        public int UserId { get; set; }
        public DateTime? JoinedAt { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class ConversationDocument
    {
        // This maps to _id and matches your original repo
        [BsonId]
        public int ConversationId { get; set; }

        public int? TripDestinationId { get; set; }
        public bool IsGroup { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsArchived { get; set; }
        public List<ConversationParticipantEmbedded> Participants { get; set; } = new();
    }

    [BsonIgnoreExtraElements]
    internal class MessageDocument
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? SentAt { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class TripDestinationNameDocument
    {
        public int TripDestinationId { get; set; }
        public int DestinationId { get; set; }
    }

    [BsonIgnoreExtraElements]
    internal class DestinationNameDocument
    {
        public int DestinationId { get; set; }
        public string Name { get; set; } = null!;
    }

    // -----------------------------
    // Repository implementation
    // -----------------------------

    public class MongoDbMessagingRepository : IMessagingRepository
    {
        private readonly IMongoCollection<ConversationDocument> _conversationCollection;
        private readonly IMongoCollection<MessageDocument> _messageCollection;
        private readonly IMongoCollection<UserDocument> _userCollection;
        private readonly IMongoCollection<TripDestinationNameDocument> _tripDestNameCollection;
        private readonly IMongoCollection<DestinationNameDocument> _destinationCollection;
        private readonly IMongoCollection<TripDocument> _tripCollection;

        public MongoDbMessagingRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _conversationCollection = database.GetCollection<ConversationDocument>("conversations");
            _messageCollection      = database.GetCollection<MessageDocument>("messages");
            _userCollection         = database.GetCollection<UserDocument>("users");
            _tripDestNameCollection = database.GetCollection<TripDestinationNameDocument>("trips");
            _destinationCollection  = database.GetCollection<DestinationNameDocument>("destinations");
            _tripCollection         = database.GetCollection<TripDocument>("trips");
        }

        // -----------------------------
        // Mapping helpers
        // -----------------------------

        private static ConversationOverview MapConversationOverview(
            ConversationDocument doc,
            MessageDocument? lastMessageDoc,
            int currentUserId,
            DestinationNameDocument? destination = null,
            IEnumerable<UserDocument>? users = null)
        {
            string conversationName;

            if (doc.IsGroup)
            {
                conversationName = destination != null
                    ? $"Group for {destination.Name}"
                    : "Group Conversation";
            }
            else
            {
                var otherUserId = doc.Participants
                    .FirstOrDefault(p => p.UserId != currentUserId)?.UserId;

                conversationName = users?
                    .FirstOrDefault(u => u.UserId == otherUserId)?.Name
                    ?? "Direct Message";
            }

            return new ConversationOverview
            {
                ConversationId     = doc.ConversationId,
                TripDestinationId  = doc.TripDestinationId,
                IsGroup            = doc.IsGroup,
                CreatedAt          = doc.CreatedAt,
                IsArchived         = doc.IsArchived,
                ParticipantCount   = doc.Participants.Count,
                LastMessagePreview = lastMessageDoc?.Content,
                LastMessageAt      = lastMessageDoc?.SentAt,
                ConversationName   = conversationName
            };
        }

        private async Task<Conversation?> MapConversationWithUsersAsync(ConversationDocument? doc)
        {
            if (doc == null)
                return null;

            var userIds = doc.Participants.Select(p => p.UserId).Distinct().ToList();

            var userDocs = await _userCollection
                .Find(Builders<UserDocument>.Filter.In(u => u.UserId, userIds))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.UserId, u => u);

            var conversation = new Conversation
            {
                ConversationId           = doc.ConversationId,
                TripDestinationId        = doc.TripDestinationId,
                IsGroup                  = doc.IsGroup,
                CreatedAt                = doc.CreatedAt,
                IsArchived               = doc.IsArchived,
                ConversationParticipants = new List<ConversationParticipant>(),
                Messages                 = new List<Message>()
            };

            foreach (var p in doc.Participants)
            {
                userMap.TryGetValue(p.UserId, out var uDoc);

                conversation.ConversationParticipants.Add(new ConversationParticipant
                {
                    ConversationId = conversation.ConversationId,
                    UserId         = p.UserId,
                    JoinedAt       = p.JoinedAt,
                    Conversation   = conversation,
                    User = new User
                    {
                        UserId = p.UserId,
                        Name   = uDoc?.Name  ?? string.Empty,
                        Email  = uDoc?.Email ?? string.Empty
                    }
                });
            }

            return conversation;
        }

        private async Task<int> GetNextMessageIdAsync()
        {
            var last = await _messageCollection
                .Find(FilterDefinition<MessageDocument>.Empty)
                .SortByDescending(m => m.MessageId)
                .Limit(1)
                .FirstOrDefaultAsync();

            return (last?.MessageId ?? 0) + 1;
        }

        // -----------------------------
        // CRUD operations
        // -----------------------------

        public async Task<IEnumerable<ConversationOverview>> GetConversationsForUserAsync(int userId)
        {
            // 1. Find conversations for this user
            var filter = Builders<ConversationDocument>.Filter
                .ElemMatch(c => c.Participants, p => p.UserId == userId);

            var docs = await _conversationCollection.Find(filter).ToListAsync();
            var conversationIds = docs.Select(d => d.ConversationId).ToList();

            // 2. Find last message per conversation
            var lastMessages = await _messageCollection
                .Aggregate()
                .Match(m => conversationIds.Contains(m.ConversationId))
                .SortByDescending(m => m.SentAt)
                .Group(m => m.ConversationId,
                    g => g.First())
                .ToListAsync();

            var lastMessageByConvId = lastMessages.ToDictionary(m => m.ConversationId);

            // 3. Load destinations and users
            var tripDestIds = docs
                .Where(d => d.TripDestinationId.HasValue)
                .Select(d => d.TripDestinationId)
                .ToList();

            var tripDestDocs = await _tripDestNameCollection
                .Find(td => tripDestIds.Contains(td.TripDestinationId))
                .ToListAsync();

            var destinationIds = tripDestDocs.Select(td => td.DestinationId).ToList();

            var destinations = await _destinationCollection
                .Find(d => destinationIds.Contains(d.DestinationId))
                .ToListAsync();

            var destById     = destinations.ToDictionary(d => d.DestinationId);
            var tripDestById = tripDestDocs.ToDictionary(td => td.TripDestinationId);

            var users = await _userCollection.Find(_ => true).ToListAsync();

            // 4. Map to overviews
            return docs.Select(doc =>
            {
                lastMessageByConvId.TryGetValue(doc.ConversationId, out var lastMsg);

                DestinationNameDocument? destination = null;
                if (doc.TripDestinationId.HasValue &&
                    tripDestById.TryGetValue(doc.TripDestinationId.Value, out var tripDest) &&
                    destById.TryGetValue(tripDest.DestinationId, out var dest))
                {
                    destination = dest;
                }

                return MapConversationOverview(doc, lastMsg, userId, destination, users);
            }).ToList();
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateConversationAsync(CreateConversationDto createConversationDto)
        {
            try
            {
                // 1. PRIVATE CONVERSATION LOGIC
                if (!createConversationDto.IsGroup && createConversationDto.OtherUserId != null)
                {
                    int ownerId = createConversationDto.OwnerId;
                    int otherUserId = createConversationDto.OtherUserId.Value;
                    int? tripDestId = createConversationDto.TripDestinationId;

                    if (ownerId == otherUserId)
                    {
                        return (false, "User cannot create conversation with themselves");
                    }

                    // Check if private conversation already exists
                    var filter = Builders<ConversationDocument>.Filter.And(
                        Builders<ConversationDocument>.Filter.Eq(c => c.TripDestinationId, tripDestId),
                        Builders<ConversationDocument>.Filter.Eq(c => c.IsGroup, false),
                        Builders<ConversationDocument>.Filter.Size(c => c.Participants, 2),
                        Builders<ConversationDocument>.Filter.ElemMatch(c => c.Participants, p => p.UserId == ownerId),
                        Builders<ConversationDocument>.Filter.ElemMatch(c => c.Participants, p => p.UserId == otherUserId)
                    );

                    var existingConversation = await _conversationCollection.Find(filter).FirstOrDefaultAsync();

                    if (existingConversation != null)
                    {
                        return (false, "Private conversation already exists");
                    }

                    // Create new Conversation
                    var newConversation = new ConversationDocument
                    {
                        TripDestinationId = tripDestId,
                        IsGroup = false,
                        CreatedAt = DateTime.UtcNow,
                        Participants = new List<ConversationParticipantEmbedded>
                        {
                            new ConversationParticipantEmbedded { UserId = ownerId, JoinedAt = DateTime.UtcNow },
                            new ConversationParticipantEmbedded { UserId = otherUserId, JoinedAt = DateTime.UtcNow }
                        }
                    };

                    await _conversationCollection.InsertOneAsync(newConversation);
                    return (true, null);
                }
                // 2. GROUP CONVERSATION LOGIC
                else if (createConversationDto.IsGroup && createConversationDto.TripDestinationId != null)
                {
                    int ownerId = createConversationDto.OwnerId;
                    int? tripDestIdNullable = createConversationDto.TripDestinationId;
                    if (!tripDestIdNullable.HasValue)
                    {
                        return (false, "Trip destination ID is required for group conversation.");
                    }
                    int tripDestId = tripDestIdNullable.Value;

                    // Check if group conversation already exists for this trip destination
                    var existingGroupFilter = Builders<ConversationDocument>.Filter.And(
                        Builders<ConversationDocument>.Filter.Eq(c => c.IsGroup, true),
                        Builders<ConversationDocument>.Filter.Eq(c => c.TripDestinationId, tripDestId)
                    );

                    var existingGroup = await _conversationCollection.Find(existingGroupFilter).FirstOrDefaultAsync();
                    if (existingGroup != null)
                    {
                        return (true, null);
                    }

                    // Validation - Verify Trip Owner
                    var tripDoc = await _tripCollection
                        .Find(Builders<TripDocument>.Filter.ElemMatch(
                            "Destinations", Builders<TripDestinationEmbedded>.Filter.Eq(d => d.TripDestinationId, tripDestId)))
                        .FirstOrDefaultAsync();

                    if (tripDoc == null)
                    {
                        return (false, "Trip not found for this destination.");
                    }
                    if (tripDoc.OwnerId != ownerId)
                    {
                        return (false, "Permission denied: Only the trip owner can create the group conversation.");
                    }

                    // Create the Group Conversation
                    var newGroupConversation = new ConversationDocument
                    {
                        TripDestinationId = tripDestId,
                        IsGroup = true,
                        CreatedAt = DateTime.UtcNow,
                        Participants = new List<ConversationParticipantEmbedded>
                        {
                            new ConversationParticipantEmbedded { UserId = ownerId, JoinedAt = DateTime.UtcNow }
                        }
                    };

                    await _conversationCollection.InsertOneAsync(newGroupConversation);
                    return (true, null);
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
        }

        public async Task<Conversation?> GetConversationParticipantAsync(int conversationId)
        {
            var doc = await _conversationCollection
                .Find(c => c.ConversationId == conversationId)
                .FirstOrDefaultAsync();

            return await MapConversationWithUsersAsync(doc);
        }

        public async Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId)
        {
            var msgDocs = await _messageCollection
                .Find(m => m.ConversationId == conversationId)
                .SortBy(m => m.SentAt)
                .ToListAsync();

            var conversation = await GetConversationParticipantAsync(conversationId);

            var messages = new List<Message>();
            foreach (var m in msgDocs)
            {
                var sender = conversation?.ConversationParticipants
                    .FirstOrDefault(cp => cp.UserId == m.SenderId)?.User;

                messages.Add(new Message
                {
                    MessageId      = m.MessageId,
                    ConversationId = m.ConversationId,
                    SenderId       = m.SenderId,
                    Content        = m.Content,
                    SentAt         = m.SentAt,
                    Conversation   = conversation ?? new Conversation { ConversationId = conversationId },
                    Sender         = sender
                });
            }

            return messages;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            if (message.MessageId == 0)
            {
                message.MessageId = await GetNextMessageIdAsync();
            }

            message.SentAt ??= DateTime.UtcNow;

            var doc = new MessageDocument
            {
                MessageId      = message.MessageId,
                ConversationId = message.ConversationId,
                SenderId       = message.SenderId ?? 0,
                Content        = message.Content,
                SentAt         = message.SentAt
            };

            await _messageCollection.InsertOneAsync(doc);
            return message;
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<ConversationAudit>> GetConversationAuditsAsync()
        {
            // TODO: Implement if it actually has audit table in MongoDB
            return await Task.FromResult(Enumerable.Empty<ConversationAudit>());
        }
    }
}