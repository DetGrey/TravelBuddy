using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Messaging;
using TravelBuddy.Api.Auth;

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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ConversationSummaryDto>>> GetForUser(
            [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var result = await _messagingService.GetConversationsForUserAsync(userId);
            if (result == null || !result.Any())
                return NoContent();
            
            return Ok(result);
        }

        // -------------------------------------------------------
        // 2) GET conversation details (participants + metadata)
        //    /api/messaging/{conversationId}?userId=26
        // -------------------------------------------------------

        [Authorize]
        [HttpGet("{conversationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ConversationDetailDto>> GetConversation(
            int conversationId,
            [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();
            
            var result = await _messagingService.GetConversationDetailAsync(userId, conversationId);

            if (result == null)
                return NoContent();
            
            return Ok(result);
        }

        // -------------------------------------------------------
        // 3) GET all messages in a conversation  
        //    /api/messaging/{conversationId}/messages?userId=26
        // -------------------------------------------------------
        [Authorize]
        [HttpGet("{conversationId:int}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<MessageDto>>>
            GetMessages(int conversationId, [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var result = await _messagingService.GetMessagesForConversationAsync(userId, conversationId);

            if (result == null || !result.Any())
                return NoContent();
            
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
        [Authorize]
        [HttpPost("{conversationId:int}/messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<MessageDto>> SendMessage(
            int conversationId,
            [FromQuery] int userId,
            [FromBody] SendMessageRequestDto request)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            if (string.IsNullOrWhiteSpace(request.Content))
            {
                return BadRequest("Content must not be empty.");
            }

            var result = await _messagingService.SendMessageAsync(userId,conversationId,request.Content);

            if (result == null)
            {
                return NoContent();
            }

            return Ok(result);
        }

    }
}