using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Messaging.Infrastructure;
using System.Runtime.CompilerServices;

namespace TravelBuddy.Messaging
{
    public interface IMessagingRepository
    {
        Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId);

        Task<Conversation?> GetConversationParticipantAsync(int conversationId);
    }

    public class MessagingRepository : IMessagingRepository
    {
        private readonly MessagingDbContext _context;

        public MessagingRepository(MessagingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
        {
            return await _context.Conversations
                .Include(c => c.ConversationParticipants)
                .Where(c => c.ConversationParticipants.Any(cp => cp.UserId == userId))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Conversation?> GetConversationParticipantAsync(int conversationId)
        {
            return await _context.Conversations
                .Include(c => c.ConversationParticipants)
                    .ThenInclude(cp => cp.User)
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId); 
        }
    }
}