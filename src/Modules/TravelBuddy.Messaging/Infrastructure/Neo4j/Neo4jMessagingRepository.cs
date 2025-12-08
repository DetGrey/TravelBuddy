using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Neo4j.Driver;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Users.Models;
using TravelBuddy.Trips.Models;

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
                OPTIONAL MATCH (t:Trip)-[hs:HAS_STOP { tripDestinationId: c.tripDestinationId }]->(d:Destination)
                WITH u, c, collect(DISTINCT p) AS participants, m, d
                ORDER BY m.sentAt DESC
                WITH u, c, participants, head(collect(m)) AS lastMessage, d
                RETURN c, participants, lastMessage, d
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
                var destinationNode  = record["d"].As<INode?>();

                var conversationId    = ReadInt(cNode, "conversationId");
                var tripDestinationId = ReadNullableInt(cNode, "tripDestinationId");
                var isGroup           = ReadBool(cNode, "isGroup");
                var isArchived        = ReadBool(cNode, "isArchived");
                var createdAt         = ReadNullableDateTime(cNode, "createdAt");

                var participantCount = participantNodes?.Count ?? 0;

                // ConversationName logic
                string conversationName;
                if (isGroup)
                {
                    var destinationName = destinationNode != null
                        ? ReadString(destinationNode, "name")
                        : null;
                    
                    conversationName = destinationName != null
                        ? $"Group for {destinationName}"
                        : "Group Conversation";
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
                        // AND Convo is not group AND Convo has matching tripDestinationId
                        var checkQuery = @"
                            MATCH (u1:User {userId: $ownerId})
                            MATCH (u2:User {userId: $otherUserId})
                            MATCH (u1)-[:PARTICIPATES_IN]->(c:Conversation {isGroup: false, tripDestinationId: $tripDestId})
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

                        // Create Conversation and Relationships
                        // Note: TripDestination exists as properties on HAS_STOP relationships, not as nodes
                        var createQuery = @"
                            MATCH (u1:User {userId: $ownerId})
                            MATCH (u2:User {userId: $otherUserId})
                            CREATE (c:Conversation {
                                isGroup: false, 
                                createdAt: datetime(), 
                                tripDestinationId: $tripDestId,
                                conversationId: randomUUID() 
                            })
                            CREATE (u1)-[:PARTICIPATES_IN]->(c)
                            CREATE (u2)-[:PARTICIPATES_IN]->(c)
                            RETURN c.conversationId";

                        await tx.RunAsync(createQuery, new { 
                            ownerId = createConversationDto.OwnerId,
                            otherUserId = createConversationDto.OtherUserId,
                            tripDestId = createConversationDto.TripDestinationId
                        });

                        return (true, (string?)null);
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
                        // 2. Validate Owner (User -> OWNS -> Trip -[HAS_STOP {tripDestinationId}]-> Destination)
                        // 3. Create Conversation
                        // 4. Add Owner
                        // 5. Find Buddies (User -[BUDDY_ON {tripDestinationId, requestStatus:'accepted'}]-> Trip) and add them

                        // First check if group conversation already exists
                        var checkExistingQuery = @"
                            MATCH (c:Conversation {isGroup: true, tripDestinationId: $tripDestId})
                            RETURN c.conversationId";
                        
                        var existingCursor = await tx.RunAsync(checkExistingQuery, new { 
                            tripDestId = createConversationDto.TripDestinationId 
                        });

                        if (await existingCursor.FetchAsync())
                        {
                            return (true, (string?)null);
                        }

                        // Validate Owner has a Trip with a HAS_STOP relationship containing this tripDestinationId
                        var checkOwnerQuery = @"
                            MATCH (owner:User {userId: $ownerId})-[:OWNS]->(t:Trip)-[stop:HAS_STOP {tripDestinationId: $tripDestId}]->(d:Destination)
                            RETURN t.tripId";
                        
                        var ownerCheckCursor = await tx.RunAsync(checkOwnerQuery, new { 
                            ownerId = createConversationDto.OwnerId, 
                            tripDestId = createConversationDto.TripDestinationId 
                        });

                        if (!await ownerCheckCursor.FetchAsync())
                        {
                            return (false, "Permission denied: Only the main trip owner can create the group conversation or Trip Destination not found.");
                        }

                        // Create Conversation and add participants
                        var createQuery = @"
                            // Find the trip that has this tripDestinationId
                            MATCH (owner:User {userId: $ownerId})-[:OWNS]->(t:Trip)-[stop:HAS_STOP {tripDestinationId: $tripDestId}]->(d:Destination)
                            
                            // Create Conversation
                            CREATE (c:Conversation {
                                isGroup: true,
                                createdAt: datetime(),
                                tripDestinationId: $tripDestId,
                                conversationId: randomUUID()
                            })
                            CREATE (owner)-[:PARTICIPATES_IN]->(c)

                            // Find Accepted Buddies for this tripDestinationId and add them
                            WITH c, t, $ownerId AS ownerId
                            MATCH (buddy:User)-[buddyRel:BUDDY_ON {tripDestinationId: $tripDestId}]->(t)
                            WHERE buddyRel.requestStatus = 'accepted' AND buddy.userId <> ownerId
                            CREATE (buddy)-[:PARTICIPATES_IN]->(c)
                            
                            RETURN c.conversationId";

                        await tx.RunAsync(createQuery, new { 
                            ownerId = createConversationDto.OwnerId, 
                            tripDestId = createConversationDto.TripDestinationId 
                        });

                        return (true, (string?)null);
                    });
                }

                return (true, (string?)null);
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
                OPTIONAL MATCH (t:Trip)-[hs:HAS_STOP { tripDestinationId: c.tripDestinationId }]->(d:Destination)
                RETURN c, collect(DISTINCT u) AS participants, d.name AS destinationName
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
            var destinationName  = record["destinationName"].As<string?>();

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

            // Create TripDestination if we have a destination name (for group conversations)
            TripDestination? tripDestination = null;
            if (tripDestinationId.HasValue && !string.IsNullOrEmpty(destinationName))
            {
                tripDestination = new TripDestination
                {
                    TripDestinationId = tripDestinationId.Value,
                    Destination = new Destination
                    {
                        Name = destinationName
                    }
                };
            }

            var conversation = new Conversation
            {
                ConversationId           = convId,
                TripDestinationId        = tripDestinationId,
                IsGroup                  = isGroup,
                IsArchived               = isArchived,
                CreatedAt                = createdAt,
                ConversationParticipants = participants,
                Messages                 = new List<Message>(),
                TripDestination          = tripDestination
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

        public async Task<(bool Success, string? ErrorMessage)> DeleteConversationAsync(int conversationId, int changedBy)
        {
            await using var session = _driver.AsyncSession();
            
            try
            {
                return await session.ExecuteWriteAsync(async tx =>
                {
                    // Check if conversation exists
                    var checkCypher = @"
                        MATCH (c:Conversation {conversationId: $conversationId})
                        RETURN c.conversationId as ConversationId
                    ";
                    var checkCursor = await tx.RunAsync(checkCypher, new { conversationId });
                    var checkRecords = await checkCursor.ToListAsync();
                    
                    if (!checkRecords.Any())
                        return (false, "No conversation found with the given conversation_id");

                    // PERMANENTLY delete the conversation and all relationships
                    var deleteCypher = @"
                        MATCH (c:Conversation {conversationId: $conversationId})
                        OPTIONAL MATCH (c)-[:HAS_MESSAGE]->(m:Message)
                        DETACH DELETE m, c
                    ";
                    await tx.RunAsync(deleteCypher, new { conversationId });

                    return (true, (string?)null);
                });
            }
            catch (Exception ex)
            {
                return (false, "Error: " + ex.Message);
            }
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<ConversationAudit>> GetConversationAuditsAsync()
        {
            await using var session = _driver.AsyncSession();
            const string cypher = @"
                MATCH (c:Conversation)-[:HAS_AUDIT]->(a:ConversationAudit)
                OPTIONAL MATCH (cb:User)-[:CHANGED]->(a)
                OPTIONAL MATCH (aff:User)-[:AFFECTED_BY]->(a)
                RETURN a.auditId as AuditId, c.conversationId as ConversationId,
                       aff.userId as AffectedUserId, a.action as Action,
                       cb.userId as ChangedBy, a.timestamp as Timestamp
                ORDER BY a.auditId DESC
            ";
            
            var cursor = await session.RunAsync(cypher);
            var records = await cursor.ToListAsync();
            
            return records.Select(r => new ConversationAudit
            {
                AuditId = r["AuditId"].As<int?>() ?? 0,
                ConversationId = r["ConversationId"].As<int?>() ?? 0,
                AffectedUserId = r["AffectedUserId"].As<int?>(),
                Action = r["Action"].As<string?>() ?? string.Empty,
                ChangedBy = r["ChangedBy"].As<int?>(),
                Timestamp = ParseDateTime(r["Timestamp"])
            }).ToList();
        }
        
        private static DateTime ParseDateTime(object value)
        {
            if (value == null)
                return DateTime.MinValue;

            if (value is DateTime dt)
                return dt;

            if (value is Neo4j.Driver.ZonedDateTime zdt)
                return zdt.ToDateTimeOffset().DateTime;

            if (value is Neo4j.Driver.LocalDateTime ldt)
                return ldt.ToDateTime();

            if (value is string s && DateTime.TryParse(s, System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.AssumeUniversal, out var parsed))
                return parsed;

            return DateTime.MinValue;
        }
    }
}