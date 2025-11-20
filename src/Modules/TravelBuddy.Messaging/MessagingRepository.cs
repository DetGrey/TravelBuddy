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

        Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId);

        Task<Message> AddMessageAsync(Message message);
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

        public async Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId)
        {
            return await _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Message> AddMessageAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}