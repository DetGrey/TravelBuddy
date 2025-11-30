using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Messaging.Infrastructure;
using System.Runtime.CompilerServices;

namespace TravelBuddy.Messaging;
public interface IMessagingRepository
{
    Task<IEnumerable<ConversationOverview>> GetConversationsForUserAsync(int userId);

    Task<Conversation?> GetConversationParticipantAsync(int conversationId);

    Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId);

    Task<Message> AddMessageAsync(Message message);
}
