using Microsoft.CodeAnalysis;
using TravelBuddy.Messaging.Models;
using System.Linq;

namespace TravelBuddy.Messaging
{
    public interface IMessagingService
    {
        Task<IEnumerable<ConversationOverviewDto>> GetConversationsForUserAsync(int userId);
        Task<(bool Success, string? ErrorMessage)> CreateConversationAsync(CreateConversationDto createConversationDto);
        Task<ConversationDetailDto?> GetConversationDetailAsync(int userId, int conversationId);
        Task<IReadOnlyList<MessageDto>> GetMessagesForConversationAsync(int userId, int conversationId);

        Task<MessageDto?> SendMessageAsync(int userId, int conversationId, string content);
        
        // Admin deletion methods
        Task<(bool Success, string? ErrorMessage)> DeleteConversationAsync(int conversationId, int changedBy);

        // Audit related
        Task<IEnumerable<ConversationAuditDto>> GetConversationAuditsAsync();
    }

    public class MessagingService : IMessagingService
    {
        private readonly IMessagingRepositoryFactory _messagingRepositoryFactory;

        public MessagingService(IMessagingRepositoryFactory messagingRepositoryFactory)
        {
            _messagingRepositoryFactory = messagingRepositoryFactory;
        }

        // Helper method to get the correct repository for the current request scope
        private IMessagingRepository GetRepo() => _messagingRepositoryFactory.GetMessagingRepository();

        public async Task<IEnumerable<ConversationOverviewDto>> GetConversationsForUserAsync(int userId)
        {
            var messagingRepository = GetRepo();
            var conversations = await messagingRepository.GetConversationsForUserAsync(userId);

            return conversations 
                .Select(c => new ConversationOverviewDto(
                    ConversationId: c.ConversationId,
                    TripDestinationId: c.TripDestinationId,
                    IsGroup: c.IsGroup,
                    CreatedAt: c.CreatedAt,
                    IsArchived: c.IsArchived,
                    ParticipantCount: c.ParticipantCount,
                    LastMessagePreview: c.LastMessagePreview,
                    LastMessageAt: c.LastMessageAt,
                    ConversationName: c.ConversationName
                ))
                .ToList();
        }
        public async Task<(bool Success, string? ErrorMessage)> CreateConversationAsync(CreateConversationDto createConversationDto)
        {
            var isPrivate = !createConversationDto.IsGroup && createConversationDto.OtherUserId != null;
            var isTripGroup = createConversationDto.IsGroup && createConversationDto.TripDestinationId != null;
            if (!isPrivate && !isTripGroup)
                return (false, "Should be either a private conversation or a trip group");
            
            var messagingRepository = GetRepo();
            return await messagingRepository.CreateConversationAsync(createConversationDto);
        }
        public async Task<ConversationDetailDto?> GetConversationDetailAsync(int userId, int conversationId)
        {
            var messagingRepository = GetRepo();
            var conversation = await messagingRepository.GetConversationParticipantAsync(conversationId);

            if (conversation == null)
                return null;
            
            if (!conversation.ConversationParticipants.Any(cp => cp.UserId == userId))
                return null;
            
            var participants = conversation.ConversationParticipants
                .Select(p => new ConversationParticipantDto(
                    UserId: p.UserId,
                    Name: p.User.Name,
                    Email: p.User.Email
                ))
                .ToList();

            return new ConversationDetailDto(
                Id: conversation.ConversationId,
                IsGroup: conversation.IsGroup,
                IsArchived: conversation.IsArchived,
                CreatedAt: conversation.CreatedAt,
                ParticipantCount: participants.Count,
                Participant: participants
            );
        }

        public async Task<IReadOnlyList<MessageDto>> GetMessagesForConversationAsync(int userId, int conversationId)
        {
            var messagingRepository = GetRepo();
            // 1. Checks if the conversation exist and user is a participant
            var conversation = await messagingRepository.GetConversationParticipantAsync(conversationId);
            if (conversation == null)
                return Array.Empty<MessageDto>();
            
            var isParticipant = conversation.ConversationParticipants
                .Any(cp => cp.UserId == userId);
            
            if (!isParticipant)
            {
                return Array.Empty<MessageDto>();   
            }
            
            // 2. Retrieve all messages in the conversation
            var messages = await messagingRepository.GetMessagesForConversationAsync(conversationId);

            // 3. Maps to DTO'er
            return messages
                .Select(m => new MessageDto(
                    Id: m.MessageId,
                    ConversationId: m.ConversationId,
                    SenderId: m.SenderId,
                    SenderName: m.Sender?.Name,
                    Content: m.Content,
                    SentAt: m.SentAt
            ))
            .ToList();

        }

        public async Task<MessageDto?> SendMessageAsync(int userId, int conversationId, string content)
        {
            var messagingRepository = GetRepo();
            // 1. Is there a conversation?
            var conversation = await messagingRepository.GetConversationParticipantAsync(conversationId);
            if (conversation == null)
            {
                return null;
            }

            // 2. Is the user a participant in the conversation?
            var isParticipant = conversation.ConversationParticipants
                .Any(cp => cp.UserId == userId);
            
            if (!isParticipant)
            {
                return null; 
            }

            // 3. Create a new message_entity
            var message = new Message
            {
                ConversationId = conversationId,
                SenderId       = userId,
                Content        = content,
                SentAt         = DateTime.UtcNow
            };

            // 4. Save the message in the DB
            var saved = await messagingRepository.AddMessageAsync(message);

            // 5. Look up the sender in the conversation participants (works for MySQL, Mongo, Neo4j)
            var senderUser = conversation.ConversationParticipants
                .FirstOrDefault(cp => cp.UserId == userId)?
                .User;

            var senderName = senderUser?.Name;

            // 6. Map to DTO with SenderName filled in
            return saved is null ? null : new MessageDto(
                Id:            saved.MessageId,
                ConversationId: saved.ConversationId,
                SenderId:      saved.SenderId,
                SenderName:    senderName,
                Content:       saved.Content,
                SentAt:        saved.SentAt
            );
        }

        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<(bool Success, string? ErrorMessage)> DeleteConversationAsync(int conversationId, int changedBy)
        {
            var messagingRepository = GetRepo();
            return await messagingRepository.DeleteConversationAsync(conversationId, changedBy);
        }

        public async Task<IEnumerable<ConversationAuditDto>> GetConversationAuditsAsync()
        {
            var messagingRepository = GetRepo();
            var audits = await messagingRepository.GetConversationAuditsAsync();
            return audits.Select(ca => new ConversationAuditDto(
                ca.AuditId,
                ca.ConversationId,
                ca.AffectedUserId,
                ca.Action,
                ca.ChangedBy,
                ca.Timestamp
            )).ToList();
        }
    }
}