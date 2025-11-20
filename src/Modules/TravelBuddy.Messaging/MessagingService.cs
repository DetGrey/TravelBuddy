using Microsoft.CodeAnalysis;
using TravelBuddy.Messaging.Models;
using System.Linq;

namespace TravelBuddy.Messaging
{
    public interface IMessagingService
    {
        Task<IEnumerable<ConversationSummaryDto>> GetConversationsForUserAsync(int userId);

        Task<ConversationDetailDto?> GetConversationDetailAsync(int userId, int conversationId);

        Task<IReadOnlyList<MessageDto>> GetMessagesForConversationAsync(int userId, int conversationId);

        Task<MessageDto> SendMessageAsync(int userId, int conversationId, string content);
    }

    public class MessagingService : IMessagingService
    {
        private readonly IMessagingRepository _messagingRepository;

        public MessagingService(IMessagingRepository messagingRepository)
        {
            _messagingRepository = messagingRepository;
        }

        public async Task<IEnumerable<ConversationSummaryDto>> GetConversationsForUserAsync(int userId)
        {
            var conversations = await _messagingRepository.GetConversationsForUserAsync(userId);

            return conversations 
                .Select(c => new ConversationSummaryDto(
                    Id: c.ConversationId,
                    IsGroup: c.IsGroup,
                    IsArchived: c.IsArchived,
                    ParticipantCount: c.ConversationParticipants.Count,
                    CreatedAt: c.CreatedAt
                ))
                .ToList();
        }

        public async Task<ConversationDetailDto?> GetConversationDetailAsync(int userId, int conversationId)
        {
            var conversation = await _messagingRepository.GetConversationParticipantAsync(conversationId);

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
            // 1. Checks if the conversation exist and user is a participant
            var conversation = await _messagingRepository.GetConversationParticipantAsync(conversationId);
            if (conversation == null)
                return Array.Empty<MessageDto>();
            
            var isParticipant = conversation.ConversationParticipants
                .Any(cp => cp.UserId == userId);
            
            if (!isParticipant)
            {
                return Array.Empty<MessageDto>();   
            }
            
            // 2. Retrieve all messages in the conversation
            var messages = await _messagingRepository.GetMessagesForConversationAsync(conversationId);

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
            // 1. Is there a conversation?
            var conversation = await _messagingRepository.GetConversationParticipantAsync(conversationId);
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
                SenderId = userId,
                Content = content,
                SentAt = DateTime.UtcNow
            };

            // 4. Save the message in the DB
            var saved = await _messagingRepository.AddMessageAsync(message);

            // 5. Maps to DTO
            return new MessageDto(
                Id: saved.MessageId,
                ConversationId: saved.ConversationId,
                SenderId: saved.SenderId,
                SenderName: null,
                Content: saved.Content,
                SentAt: saved.SentAt
            );
        }
    }
}