using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Messaging;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    public class MessagingController : ControllerBase
    {
        private readonly IMessagingService _messagingService;

        public MessagingController(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        // -------------------------------------------------------
        // 1) GET all conversations for a user
        //    /api/messaging?userId=26
        // -------------------------------------------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationSummaryDto>>> GetForUser(
            [FromQuery] int userId)
        {
            var result = await _messagingService.GetConversationsForUserAsync(userId);
            return Ok(result);
        }

        // -------------------------------------------------------
        // 2) GET conversation details (participants + metadata)
        //    /api/messaging/{conversationId}?userId=26
        // -------------------------------------------------------

        [HttpGet("{conversationId}")]
        public async Task<ActionResult<ConversationDetailDto>> GetConversation(
            int conversationId,
            [FromQuery] int userId)
        {
            var result = await _messagingService.GetConversationDetailAsync(userId, conversationId);

            if (result == null)
                return NotFound();
            
            return Ok(result);
        }

        // -------------------------------------------------------
        // 3) GET all messages in a conversation  
        //    /api/messaging/{conversationId}/messages?userId=26
        // -------------------------------------------------------

        [HttpGet("{conversationId:int}/messages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>>
            GetMessages(int conversationId, [FromQuery] int userId)
        {
            var result = await _messagingService.GetMessagesForConversationAsync(userId, conversationId);

            return Ok(result);
        }
    }
}