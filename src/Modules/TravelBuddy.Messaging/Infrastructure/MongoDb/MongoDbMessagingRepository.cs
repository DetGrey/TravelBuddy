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
        [BsonElement("LegacyUserId")]
        public int LegacyUserId { get; set; }
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
        // This property now maps to MongoDB's _id field
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
        public int MessageId { get; set; }          // stays as-is
        public int ConversationId { get; set; }
        public int SenderId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime? SentAt { get; set; }
    }

    // -----------------------------
    // Repository implementation
    // -----------------------------

    public class MongoDbMessagingRepository : IMessagingRepository
    {
        private readonly IMongoCollection<ConversationDocument> _conversationCollection;
        private readonly IMongoCollection<MessageDocument> _messageCollection;
        private readonly IMongoCollection<UserDocument> _userCollection;

        public MongoDbMessagingRepository(IMongoClient client)
        {
            var database = client.GetDatabase("travel_buddy_mongo");
            _conversationCollection = database.GetCollection<ConversationDocument>("conversations");
            _messageCollection = database.GetCollection<MessageDocument>("messages");
            _userCollection = database.GetCollection<UserDocument>("users");
        }

        // -----------------------------
        // Mapping helpers
        // -----------------------------

        private static Conversation MapConversationBasic(ConversationDocument doc)
        {
            var conversation = new Conversation
            {
                ConversationId = doc.ConversationId,
                TripDestinationId = doc.TripDestinationId,
                IsGroup = doc.IsGroup,
                CreatedAt = doc.CreatedAt,
                IsArchived = doc.IsArchived,
                ConversationParticipants = new List<ConversationParticipant>(),
                Messages = new List<Message>()
            };

            foreach (var p in doc.Participants)
            {
                conversation.ConversationParticipants.Add(new ConversationParticipant
                {
                    ConversationId = conversation.ConversationId,
                    UserId = p.UserId,
                    JoinedAt = p.JoinedAt,
                    Conversation = conversation,
                    User = new User
                    {
                        UserId = p.UserId,
                        Name = string.Empty,
                        Email = string.Empty
                    }
                });
            }

            return conversation;
        }

        private async Task<Conversation?> MapConversationWithUsersAsync(ConversationDocument? doc)
        {
            if (doc == null)
                return null;

            var userIds = doc.Participants.Select(p => p.UserId).Distinct().ToList();

            var userDocs = await _userCollection
                .Find(Builders<UserDocument>.Filter.In(u => u.LegacyUserId, userIds))
                .ToListAsync();

            var userMap = userDocs.ToDictionary(u => u.LegacyUserId, u => u);

            var conversation = new Conversation
            {
                ConversationId = doc.ConversationId,
                TripDestinationId = doc.TripDestinationId,
                IsGroup = doc.IsGroup,
                CreatedAt = doc.CreatedAt,
                IsArchived = doc.IsArchived,
                ConversationParticipants = new List<ConversationParticipant>(),
                Messages = new List<Message>()
            };

            foreach (var p in doc.Participants)
            {
                userMap.TryGetValue(p.UserId, out var uDoc);

                conversation.ConversationParticipants.Add(new ConversationParticipant
                {
                    ConversationId = conversation.ConversationId,
                    UserId = p.UserId,
                    JoinedAt = p.JoinedAt,
                    Conversation = conversation,
                    User = new User
                    {
                        UserId = p.UserId,
                        Name = uDoc?.Name ?? string.Empty,
                        Email = uDoc?.Email ?? string.Empty
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

        public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
        {
            var filter = Builders<ConversationDocument>.Filter
                .ElemMatch(c => c.Participants, p => p.UserId == userId);

            var docs = await _conversationCollection.Find(filter).ToListAsync();

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
                    MessageId = m.MessageId,
                    ConversationId = m.ConversationId,
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    Conversation = conversation ?? new Conversation { ConversationId = conversationId },
                    Sender = sender
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
                MessageId = message.MessageId,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId ?? 0,
                Content = message.Content,
                SentAt = message.SentAt
            };

            await _messageCollection.InsertOneAsync(doc);
            return message;
        }
    }
}
