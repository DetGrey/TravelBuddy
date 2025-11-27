using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging;

internal class UserDocument
    {
        public ObjectId Id { get; set; }

        // From migrator: LegacyUserId = u.UserId
        public int LegacyUserId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? Birthdate { get; set; }
        public bool IsDeleted { get; set; }
        public string Role { get; set; } = null!;
    }

    internal class ConversationParticipantEmbedded
    {
        public int UserId { get; set; }
        public DateTime? JoinedAt { get; set; }
    }

    internal class ConversationDocument
    {
        public ObjectId Id { get; set; }

        public int ConversationId { get; set; }
        public int? TripDestinationId { get; set; }
        public bool IsGroup { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsArchived { get; set; }

        public List<ConversationParticipantEmbedded> Participants { get; set; } = new();
    }

    internal class MessageDocument
    {
        public ObjectId Id { get; set; }

        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? SentAt { get; set; }
    }
public class MongoDbMessagingRepository : IMessagingRepository
{
    private readonly IMongoCollection<ConversationDocument> _conversationCollection;
        private readonly IMongoCollection<MessageDocument> _messageCollection;
        private readonly IMongoCollection<UserDocument> _userCollection;

    public MongoDbMessagingRepository(IMongoClient client)
    {
        var database = client.GetDatabase("travel_buddy_mongo");
        _conversationCollection = database.GetCollection<ConversationDocument>("conversations");
        _messageCollection      = database.GetCollection<MessageDocument>("messages");
        _userCollection         = database.GetCollection<UserDocument>("users");
    }

    // ---------- Helper mappers ----------

        private static Conversation MapConversationBasic(ConversationDocument doc)
        {
            var conv = new Conversation
            {
                ConversationId    = doc.ConversationId,
                TripDestinationId = doc.TripDestinationId,
                IsGroup           = doc.IsGroup,
                CreatedAt         = doc.CreatedAt,
                IsArchived        = doc.IsArchived,
                ConversationParticipants = new List<ConversationParticipant>(),
                Messages = new List<Message>()
            };

            foreach (var p in doc.Participants)
            {
                var user = new User
                {
                    UserId = p.UserId,
                    // Name/Email left empty here; this is for summaries
                    Name   = string.Empty,
                    Email  = string.Empty
                };

                conv.ConversationParticipants.Add(new ConversationParticipant
                {
                    ConversationId = conv.ConversationId,
                    UserId         = p.UserId,
                    JoinedAt       = p.JoinedAt,
                    Conversation   = conv,
                    User           = user
                });
            }

            return conv;
        }

        private async Task<Conversation?> MapConversationWithUsersAsync(ConversationDocument? doc)
        {
            if (doc == null)
                return null;

            var userIds = doc.Participants
                .Select(p => p.UserId)
                .Distinct()
                .ToList();

            if (userIds.Count == 0)
            {
                // No participants – still return a conversation object
                return MapConversationBasic(doc);
            }

            var userDocs = await _userCollection
                .Find(Builders<UserDocument>.Filter.In(u => u.LegacyUserId, userIds))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.LegacyUserId, u => u);

            var conv = new Conversation
            {
                ConversationId    = doc.ConversationId,
                TripDestinationId = doc.TripDestinationId,
                IsGroup           = doc.IsGroup,
                CreatedAt         = doc.CreatedAt,
                IsArchived        = doc.IsArchived,
                ConversationParticipants = new List<ConversationParticipant>(),
                Messages = new List<Message>()
            };

            foreach (var p in doc.Participants)
            {
                userMap.TryGetValue(p.UserId, out var uDoc);

                var user = new User
                {
                    UserId = p.UserId,
                    Name   = uDoc?.Name  ?? string.Empty,
                    Email  = uDoc?.Email ?? string.Empty
                };

                conv.ConversationParticipants.Add(new ConversationParticipant
                {
                    ConversationId = conv.ConversationId,
                    UserId         = p.UserId,
                    JoinedAt       = p.JoinedAt,
                    Conversation   = conv,
                    User           = user
                });
            }

            return conv;
        }

        private async Task<int> GetNextMessageIdAsync()
        {
            // Simple, non-atomic auto-increment: last MessageId + 1
            var last = await _messageCollection
                .Find(FilterDefinition<MessageDocument>.Empty)
                .SortByDescending(m => m.MessageId)
                .Limit(1)
                .FirstOrDefaultAsync();

            return (last?.MessageId ?? 0) + 1;
        }

    public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
    {
        var filter = Builders<ConversationDocument>.Filter
                .ElemMatch(c => c.Participants, p => p.UserId == userId);

            var docs = await _conversationCollection
                .Find(filter)
                .ToListAsync();

            // For conversation summaries we don’t need user names/emails yet
            return docs.Select(MapConversationBasic).ToList();
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
        // Messages for that conversation
            var msgDocs = await _messageCollection
                .Find(m => m.ConversationId == conversationId)
                .SortBy(m => m.SentAt)
                .ToListAsync();

            // Load conversation (with participants + users) so we can attach Sender
            var conversation = await GetConversationParticipantAsync(conversationId);

            var result = new List<Message>();

            foreach (var m in msgDocs)
            {
                User? senderUser = null;

                if (conversation != null)
                {
                    senderUser = conversation.ConversationParticipants
                        .FirstOrDefault(cp => cp.UserId == m.SenderId)
                        ?.User;
                }

                var message = new Message
                {
                    MessageId      = m.MessageId,
                    ConversationId = m.ConversationId,
                    SenderId       = m.SenderId,
                    Content        = m.Content,
                    SentAt         = m.SentAt,
                    Conversation   = conversation ?? new Conversation { ConversationId = conversationId },
                    Sender         = senderUser
                };

                result.Add(message);
            }

            return result;
    }

    public async Task<Message> AddMessageAsync(Message message)
    {
        // Assign MessageId if not set
            if (message.MessageId == 0)
            {
                message.MessageId = await GetNextMessageIdAsync();
            }

            // Ensure timestamp
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

            // MessagingService already has the Message entity and uses it to build the DTO
            return message;
    }
}
