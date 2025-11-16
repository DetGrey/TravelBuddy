using TravelBuddy.Messaging.Models;
namespace TravelBuddy.Messaging
{
    public interface IMessagingService
    {
        Task<IEnumerable<ConversationSummaryDto>> GetConversationsForUserAsync(int userId);
    }

    public class MessagingService : IMessagingService
    {
        private readonly IConversationRepository _conversationRepository;

        public MessagingService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<IEnumerable<ConversationSummaryDto>> GetConversationsForUserAsync(int userId)
        {
            var conversations = await _conversationRepository.GetConversationsForUserAsync(userId);

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
    }
}