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
    }
}