using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging
{
    /// <summary>
    /// IMessagingRepository implementation backed by Neo4j.
    ///
    /// Graph model:
    ///   (:User { userId })
    ///   (:Conversation { conversationId, tripDestinationId, isGroup, isArchived, createdAt })
    ///   (:Message { messageId, conversationId?, senderId?, content, sentAt })
    ///
    ///   (:User)-[:PARTICIPATES_IN]->(:Conversation)
    ///   (:Conversation)-[:HAS_MESSAGE]->(:Message)
    ///   (:User)-[:SENT]->(:Message)
    /// </summary>
    public sealed class Neo4jMessagingRepository : IMessagingRepository
    {
        private readonly IDriver _driver;

        public Neo4jMessagingRepository(IDriver driver)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        // ---------- Helper readers ----------

        private static int ReadInt(INode node, string key, int defaultValue = 0)
        {
            if (!node.Properties.TryGetValue(key, out var value) || value is null)
                return defaultValue;

            return value switch
            {
                int i => i,
                long l => (int)l,
                string s when int.TryParse(s, out var i) => i,
                _ => defaultValue
            };
        }

        private static int? ReadNullableInt(INode node, string key)
        {
            if (!node.Properties.TryGetValue(key, out var value) || value is null)
                return null;

            return value switch
            {
                int i => i,
                long l => (int)l,
                string s when int.TryParse(s, out var i) => i,
                _ => (int?)null
            };
        }

        private static bool ReadBool(INode node, string key, bool defaultValue = false)
        {
            if (!node.Properties.TryGetValue(key, out var value) || value is null)
                return defaultValue;

            return value switch
            {
                bool b => b,
                string s when bool.TryParse(s, out var b) => b,
                _ => defaultValue
            };
        }

        private static DateTime? ReadNullableDateTime(INode node, string key)
        {
            if (!node.Properties.TryGetValue(key, out var value) || value is null)
                return null;

            // Handle Neo4j temporal types + DateTime + string
            return value switch
            {
                DateTime dt => dt,
                Neo4j.Driver.LocalDateTime ldt => ldt.ToDateTime(),
                Neo4j.Driver.ZonedDateTime zdt => zdt.UtcDateTime,
                string s when DateTime.TryParse(s, out var dt) => dt,
                _ => (DateTime?)null
            };
        }

        private static string? ReadString(INode node, string key)
        {
            if (!node.Properties.TryGetValue(key, out var value) || value is null)
                return null;

            return value.ToString();
        }

        // ---------- IMessagingRepository implementation ----------

        /// <summary>
        /// Returns conversation overviews similar to MySqlMessagingRepository / MongoDbMessagingRepository.
        /// </summary>
        public async Task<IEnumerable<ConversationOverview>> GetConversationsForUserAsync(int userId)
        {
            const string cypher = @"
                MATCH (u:User { userId: $userId })-[:PARTICIPATES_IN]->(c:Conversation)
                OPTIONAL MATCH (c)<-[:PARTICIPATES_IN]-(p:User)
                OPTIONAL MATCH (c)-[:HAS_MESSAGE]->(m:Message)
                WITH u, c, collect(DISTINCT p) AS participants, m
                ORDER BY m.sentAt DESC
                WITH u, c, participants, head(collect(m)) AS lastMessage
                RETURN c, participants, lastMessage
                ORDER BY c.createdAt
            ";

            var overviews = new List<ConversationOverview>();

            await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var cursor  = await session.RunAsync(cypher, new { userId });
            var records = await cursor.ToListAsync();

            foreach (var record in records)
            {
                var cNode            = record["c"].As<INode>();
                var participantNodes = record["participants"].As<List<INode>>();
                var lastMsgNode      = record["lastMessage"].As<INode?>();

                var conversationId    = ReadInt(cNode, "conversationId");
                var tripDestinationId = ReadNullableInt(cNode, "tripDestinationId");
                var isGroup           = ReadBool(cNode, "isGroup");
                var isArchived        = ReadBool(cNode, "isArchived");
                var createdAt         = ReadNullableDateTime(cNode, "createdAt");

                var participantCount = participantNodes?.Count ?? 0;

                // ConversationName logic roughly aligned with Mongo:
                // - Group  => generic name for now
                // - Direct => other participant's name or "Direct Message"
                string conversationName;
                if (isGroup)
                {
                    conversationName = "Group Conversation";
                }
                else
                {
                    var otherUserNode = participantNodes?
                        .FirstOrDefault(p => ReadInt(p, "userId") != userId);

                    var otherName = otherUserNode != null
                        ? ReadString(otherUserNode, "name")
                        : null;

                    conversationName = otherName ?? "Direct Message";
                }

                string?   lastMessagePreview = null;
                DateTime? lastMessageAt      = null;

                if (lastMsgNode != null)
                {
                    lastMessagePreview = ReadString(lastMsgNode, "content");
                    lastMessageAt      = ReadNullableDateTime(lastMsgNode, "sentAt");
                }

                overviews.Add(new ConversationOverview
                {
                    ConversationId     = conversationId,
                    TripDestinationId  = tripDestinationId,
                    IsGroup            = isGroup,
                    CreatedAt          = createdAt,
                    IsArchived         = isArchived,
                    ParticipantCount   = participantCount,
                    LastMessagePreview = lastMessagePreview,
                    LastMessageAt      = lastMessageAt,
                    ConversationName   = conversationName
                });
            }

            return overviews;
        }

        public async Task<(bool Success, string? ErrorMessage)> CreateConversationAsync(CreateConversationDto createConversationDto)
        {
            return (false, null);
        }

        public async Task<Conversation?> GetConversationParticipantAsync(int conversationId)
        {
            const string cypher = @"
                MATCH (c:Conversation { conversationId: $conversationId })
                OPTIONAL MATCH (c)<-[:PARTICIPATES_IN]-(u:User)
                RETURN c, collect(u) AS participants
                LIMIT 1
            ";

            await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var cursor  = await session.RunAsync(cypher, new { conversationId });
            var records = await cursor.ToListAsync();   // at most 1 because of LIMIT 1
            var record  = records.SingleOrDefault();

            if (record is null)
                return null;

            var cNode            = record["c"].As<INode>();
            var participantNodes = record["participants"].As<List<INode>>();

            var convId            = ReadInt(cNode, "conversationId");
            var tripDestinationId = ReadNullableInt(cNode, "tripDestinationId");
            var isGroup           = ReadBool(cNode, "isGroup");
            var isArchived        = ReadBool(cNode, "isArchived");
            var createdAt         = ReadNullableDateTime(cNode, "createdAt");

            var participants = new List<ConversationParticipant>();

            foreach (var uNode in participantNodes)
            {
                if (uNode is null) continue;

                var participantUserId = ReadInt(uNode, "userId");
                var name  = ReadString(uNode, "name")  ?? string.Empty;
                var email = ReadString(uNode, "email") ?? string.Empty;

                var user = new User
                {
                    UserId = participantUserId,
                    Name   = name,
                    Email  = email
                };

                participants.Add(new ConversationParticipant
                {
                    ConversationId = convId,
                    UserId         = participantUserId,
                    JoinedAt       = null,
                    User           = user
                });
            }

            var conversation = new Conversation
            {
                ConversationId           = convId,
                TripDestinationId        = tripDestinationId,
                IsGroup                  = isGroup,
                IsArchived               = isArchived,
                CreatedAt                = createdAt,
                ConversationParticipants = participants,
                Messages                 = new List<Message>()
            };

            return conversation;
        }

        public async Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId)
        {
            const string cypher = @"
                MATCH (c:Conversation { conversationId: $conversationId })-[:HAS_MESSAGE]->(m:Message)
                OPTIONAL MATCH (m)<-[:SENT]-(u:User)
                RETURN m, u
                ORDER BY m.sentAt
            ";

            var messages = new List<Message>();

            await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Read));
            var cursor  = await session.RunAsync(cypher, new { conversationId });
            var records = await cursor.ToListAsync();

            foreach (var record in records)
            {
                var mNode = record["m"].As<INode>();
                var uNode = record["u"].As<INode?>();

                var messageId = ReadInt(mNode, "messageId");

                // Some existing seed data might not have a conversationId property on Message,
                // so we trust the method argument to align with MySQL / Mongo behavior.
                var convId = conversationId;

                // Prefer explicit senderId on the Message node; otherwise, fall back to the related User's userId.
                var senderId       = ReadNullableInt(mNode, "senderId");
                int? senderIdFinal = senderId;
                User? senderUser   = null;

                if (uNode != null)
                {
                    var uid   = ReadInt(uNode, "userId");
                    var name  = ReadString(uNode, "name")  ?? string.Empty;
                    var email = ReadString(uNode, "email") ?? string.Empty;

                    senderUser = new User
                    {
                        UserId = uid,
                        Name   = name,
                        Email  = email
                    };

                    if (senderIdFinal is null)
                    {
                        senderIdFinal = uid;
                    }
                }

                var content = ReadString(mNode, "content") ?? string.Empty;
                var sentAt  = ReadNullableDateTime(mNode, "sentAt");

                var msg = new Message
                {
                    MessageId      = messageId,
                    ConversationId = convId,
                    SenderId       = senderIdFinal,
                    Content        = content,
                    SentAt         = sentAt,
                    Sender         = senderUser
                };

                messages.Add(msg);
            }

            // List<Message> implements IReadOnlyList<Message>, so this matches the interface
            return messages;
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));

            if (message.SenderId is null)
                throw new InvalidOperationException("Message.SenderId must be set before calling AddMessageAsync.");

            await using var session = _driver.AsyncSession(o => o.WithDefaultAccessMode(AccessMode.Write));

            // Generate new messageId = max(existing) + 1
            const string maxIdCypher = @"
                MATCH (m:Message)
                RETURN coalesce(max(m.messageId), 0) AS maxId
            ";

            var maxCursor  = await session.RunAsync(maxIdCypher);
            var maxRecords = await maxCursor.ToListAsync();
            var maxRecord  = maxRecords.Single();
            var nextId     = maxRecord["maxId"].As<long>() + 1;

            var messageId = (int)nextId;
            var sentAt    = message.SentAt ?? DateTime.UtcNow;

            const string createCypher = @"
                MATCH (c:Conversation { conversationId: $conversationId })
                MATCH (u:User { userId: $senderId })
                CREATE (m:Message {
                    messageId:      $messageId,
                    conversationId: $conversationId,
                    senderId:       $senderId,
                    content:        $content,
                    sentAt:         $sentAt
                })
                MERGE (c)-[:HAS_MESSAGE]->(m)
                MERGE (u)-[:SENT]->(m)
            ";

            var parameters = new
            {
                conversationId = message.ConversationId,
                senderId       = message.SenderId.Value,
                messageId,
                content        = message.Content,
                sentAt
            };

            await session.RunAsync(createCypher, parameters);

            message.MessageId = messageId;
            message.SentAt    = sentAt;

            return message;
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<ConversationAudit>> GetConversationAuditsAsync()
        {
            // TODO: Implement if it actually has audit table in Neo4j
            return await Task.FromResult(Enumerable.Empty<ConversationAudit>());
        }
    }
}