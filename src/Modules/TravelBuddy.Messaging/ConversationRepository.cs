using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Messaging.Infrastructure;

namespace TravelBuddy.Messaging
{
    public interface IConversationRepository
    {
        Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId);
    }

    public class ConversationRepository : IConversationRepository
    {
        private readonly MessagingDbContext _context;

        public ConversationRepository(MessagingDbContext context)
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
    }
}