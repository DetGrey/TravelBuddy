using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Messaging.Infrastructure;
using System.Runtime.CompilerServices;

namespace TravelBuddy.Messaging;
public class MongoDbMessagingRepository : IMessagingRepository
{
    private readonly IMongoCollection<Conversation> _conversationCollection; // TODO update if not Trip

    public MongoDbMessagingRepository(IMongoClient client)
    {
        var database = client.GetDatabase("travel_buddy_mongo");
        _conversationCollection = database.GetCollection<Conversation>("conversations"); //TODO update
    }

    public async Task<IEnumerable<Conversation>> GetConversationsForUserAsync(int userId)
    {
        // TODO Placeholder: Return an empty collection of conversations
        return await Task.FromResult<IEnumerable<Conversation>>(new List<Conversation>());
    }

    public async Task<Conversation?> GetConversationParticipantAsync(int conversationId)
    {
        // TODO Placeholder: Return null, indicating the conversation was not found or has no participant data
        return await Task.FromResult<Conversation?>(null);
    }

    public async Task<IReadOnlyList<Message>> GetMessagesForConversationAsync(int conversationId)
    {
        // TODO Placeholder: Return an empty read-only list of messages
        return await Task.FromResult<IReadOnlyList<Message>>(new List<Message>());
    }

    public async Task<Message> AddMessageAsync(Message message)
    {
        // TODO Placeholder: Return the input message object itself (or a new default Message object)
        // In a real scenario, this would return the message *after* it's been saved and assigned an ID.
        return await Task.FromResult(message);
    }
}
