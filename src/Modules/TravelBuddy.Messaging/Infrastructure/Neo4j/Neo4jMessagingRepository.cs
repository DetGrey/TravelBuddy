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
            var session = _driver.AsyncSession();
            try
            {
                // =================================================================
                // 1. PRIVATE CONVERSATION LOGIC
                // =================================================================
                if (!createConversationDto.IsGroup && createConversationDto.OtherUserId != null)
                {
                    if (createConversationDto.OwnerId == createConversationDto.OtherUserId)
                    {
                        return (false, "User cannot create conversation with themselves");
                    }

                    return await session.ExecuteWriteAsync(async tx =>
                    {
                        // Check if private conversation already exists
                        // Pattern: (User1)-[:PARTICIPATES_IN]->(Convo)-[:PARTICIPATES_IN]-(User2)
                        // AND Convo is not group AND Convo links to TripDest
                        var checkQuery = @"
                            MATCH (u1:User {userId: $ownerId})
                            MATCH (u2:User {userId: $otherUserId})
                            MATCH (td:TripDestination {tripDestinationId: $tripDestId})
                            MATCH (u1)-[:PARTICIPATES_IN]->(c:Conversation {isGroup: false})-[:BELONGS_TO]->(td)
                            WHERE (u2)-[:PARTICIPATES_IN]->(c)
                            RETURN c.conversationId as id";

                        var cursor = await tx.RunAsync(checkQuery, new { 
                            ownerId = createConversationDto.OwnerId,
                            otherUserId = createConversationDto.OtherUserId,
                            tripDestId = createConversationDto.TripDestinationId
                        });

                        if (await cursor.FetchAsync())
                        {
                            return (false, "Private conversation already exists");
                        }

                        // Create Conversation, Relationships, and Audit Log equivalent
                        var createQuery = @"
                            MATCH (u1:User {userId: $ownerId})
                            MATCH (u2:User {userId: $otherUserId})
                            MATCH (td:TripDestination {tripDestinationId: $tripDestId})
                            CREATE (c:Conversation {
                                isGroup: false, 
                                createdAt: datetime(), 
                                tripDestinationId: $tripDestId,
                                conversationId: randomUUID() 
                            })
                            CREATE (u1)-[:PARTICIPATES_IN]->(c)
                            CREATE (u2)-[:PARTICIPATES_IN]->(c)
                            CREATE (c)-[:BELONGS_TO]->(td)
                            RETURN c.conversationId";

                        await tx.RunAsync(createQuery, new { 
                            ownerId = createConversationDto.OwnerId,
                            otherUserId = createConversationDto.OtherUserId,
                            tripDestId = createConversationDto.TripDestinationId
                        });

                        return (true, null);
                    });
                }
                // =================================================================
                // 2. GROUP CONVERSATION LOGIC
                // =================================================================
                else if (createConversationDto.IsGroup && createConversationDto.TripDestinationId != 0)
                {
                    return await session.ExecuteWriteAsync(async tx =>
                    {
                        // Logic:
                        // 1. Check if conversation exists (if so, return success)
                        // 2. Validate Owner (User -> OWNS -> Trip -> INCLUDES -> TripDestination)
                        // 3. Create Conversation
                        // 4. Add Owner
                        // 5. Find Buddies (User -> IS_BUDDY_OF {status:'accepted'} -> TripDest) and add them

                        var query = @"
                            MATCH (td:TripDestination {tripDestinationId: $tripDestId})
                            
                            // 1. Check existing Group Conversation
                            OPTIONAL MATCH (existingC:Conversation {isGroup: true})-[:BELONGS_TO]->(td)
                            WITH td, existingC
                            WHERE existingC IS NULL // Proceed only if it doesn't exist

                            // 2. Validate Owner Permissions
                            // We assume a structure: (Owner)-[:OWNS]->(Trip)-[:INCLUDES]->(TripDestination)
                            MATCH (owner:User {userId: $ownerId})
                            MATCH (owner)-[:OWNS]->(t:Trip)-[:INCLUDES]->(td)
                            
                            // 3. Create Conversation
                            CREATE (c:Conversation {
                                isGroup: true,
                                createdAt: datetime(),
                                tripDestinationId: $tripDestId,
                                conversationId: randomUUID()
                            })
                            CREATE (c)-[:BELONGS_TO]->(td)
                            CREATE (owner)-[:PARTICIPATES_IN]->(c)

                            // 4. Find Accepted Buddies and add them
                            WITH c, td
                            MATCH (buddy:User)-[r:IS_BUDDY_OF]->(td)
                            WHERE r.status = 'accepted' AND buddy.userId <> $ownerId
                            CREATE (buddy)-[:PARTICIPATES_IN]->(c)
                            
                            RETURN c.conversationId";

                        // Note: If the Owner validation fails (MATCH fails), the query creates nothing.
                        // To handle the specific error 'Permission denied' in Cypher is complex in one go.
                        // For simplicity, we run a check first.

                        var checkOwnerQuery = @"
                            MATCH (owner:User {userId: $ownerId})-[:OWNS]->(t:Trip)-[:INCLUDES]->(td:TripDestination {tripDestinationId: $tripDestId})
                            RETURN t.tripId";
                        
                        var ownerCheckCursor = await tx.RunAsync(checkOwnerQuery, new { 
                            ownerId = createConversationDto.OwnerId, 
                            tripDestId = createConversationDto.TripDestinationId 
                        });

                        if (!await ownerCheckCursor.FetchAsync())
                        {
                            return (false, "Permission denied: Only the main trip owner can create the group conversation or Trip Destination not found.");
                        }

                        // Execute main creation
                        await tx.RunAsync(query, new { 
                            ownerId = createConversationDto.OwnerId, 
                            tripDestId = createConversationDto.TripDestinationId 
                        });

                        return (true, null);
                    });
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
            finally
            {
                await session.CloseAsync();
            }
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
            await using var session = _driver.AsyncSession();
            const string cypher = @"
                MATCH (a:ConversationAudit)
                RETURN a.AuditId as AuditId, a.ConversationId as ConversationId,
                       a.AffectedUserId as AffectedUserId, a.Action as Action,
                       a.ChangedBy as ChangedBy, a.Timestamp as Timestamp
                ORDER BY a.AuditId DESC
            ";
            
            var cursor = await session.RunAsync(cypher);
            var records = await cursor.ToListAsync();
            
            return records.Select(r => new ConversationAudit
            {
                AuditId = r["AuditId"].As<int>(),
                ConversationId = r["ConversationId"].As<int>(),
                AffectedUserId = r["AffectedUserId"].As<int?>(),
                Action = r["Action"].As<string>(),
                ChangedBy = r["ChangedBy"].As<int?>(),
                Timestamp = ParseDateTime(r["Timestamp"])
            }).ToList();
        }
        
        private static DateTime ParseDateTime(object value)
        {
            if (value == null)
                throw new InvalidOperationException("Null timestamp value from Neo4j");

            if (value is DateTime dt)
                return dt;

            if (value is string s)
                return DateTime.Parse(s, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeUniversal);

            throw new InvalidOperationException($"Unsupported timestamp type: {value.GetType()}");
        }
    }
}