using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Messaging.Infrastructure;
using System.Runtime.CompilerServices;

namespace TravelBuddy.Messaging;
public class MySqlMessagingRepository : IMessagingRepository
{
    private readonly MessagingDbContext _context;

    public MySqlMessagingRepository(MessagingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ConversationOverview>> GetConversationsForUserAsync(int userId)
    {
        return await _context.ConversationOverviews
            .FromSqlInterpolated($"CALL get_user_conversations({userId})")
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

    // ------------------------------- AUDIT TABLES -------------------------------
    public async Task<IEnumerable<ConversationAudit>> GetConversationAuditsAsync()
    {
        return await _context.ConversationAudits
            .AsNoTracking()
            .ToListAsync();
    }
}
