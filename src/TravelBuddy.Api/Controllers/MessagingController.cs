using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Messaging;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    [Authorize]
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

        // -------------------------------------------------------
        // 4) POST send a message in a conversation
        //    /api/messaging/{conversationId}/messages?userId=26
        //    Body:
        //    {
        //        "content": "Hej, hvornår mødes vi?"
        //    }
        // -------------------------------------------------------

        [HttpPost("{conversationId:int}/messages")]
        public async Task<ActionResult<MessageDto>> SendMessage(
            int conversationId,
            [FromQuery] int userId,
            [FromBody] SendMessageRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Content must not be empty.");
            }

            var result = await _messagingService.SendMessageAsync(userId,conversationId,request.Content);

            if (result == null)
            {
                return NotFound("Conversation not found or you are not a partipant.");
            }

            return Ok(result);
        }

    }
}