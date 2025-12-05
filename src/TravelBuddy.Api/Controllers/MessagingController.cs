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
        [ProducesResponseType(typeof(IEnumerable<ConversationOverviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<ConversationOverviewDto>>> GetForUser(
            [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var result = await _messagingService.GetConversationsForUserAsync(userId);
            return Ok(result);
        }

        [Authorize]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> CreateConversation(
            [FromBody] CreateConversationDto createConversationDto)
        {
            if (!User.IsSelfOrAdmin(createConversationDto.OwnerId)) return Forbid();
            
            var (success, errorMessage) = await _messagingService.CreateConversationAsync(createConversationDto);

            if (!success) return BadRequest(new { error = errorMessage ?? "Failed to create conversation" });

            // Ideally return the created ID or Object here.
            return Created();
        }

        // -------------------------------------------------------
        // 2) GET conversation details (participants + metadata)
        //    /api/messaging/{conversationId}?userId=26
        // -------------------------------------------------------

        [Authorize]
        [HttpGet("{conversationId}")]
        [ProducesResponseType(typeof(ConversationDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ConversationDetailDto>> GetConversation(
            int conversationId,
            [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();
            
            var result = await _messagingService.GetConversationDetailAsync(userId, conversationId);

            if (result == null) return NotFound(new { error = "Conversation not found or access denied" });
            
            return Ok(result);
        }

        // -------------------------------------------------------
        // 3) GET all messages in a conversation  
        //    /api/messaging/{conversationId}/messages?userId=26
        // -------------------------------------------------------
        [Authorize]
        [HttpGet("{conversationId:int}/messages")]
        [ProducesResponseType(typeof(IEnumerable<MessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int conversationId, [FromQuery] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var result = await _messagingService.GetMessagesForConversationAsync(userId, conversationId);

            return Ok(result ?? new List<MessageDto>());
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
        [ProducesResponseType(typeof(MessageDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<MessageDto>> SendMessage(
            int conversationId,
            [FromQuery] int userId,
            [FromBody] SendMessageRequestDto request)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            if (string.IsNullOrWhiteSpace(request.Content)) 
                return BadRequest(new { error = "Content must not be empty." });

            var result = await _messagingService.SendMessageAsync(userId, conversationId, request.Content);

            if (result == null)
                return BadRequest(new { error = "Failed to send message" });

            return CreatedAtAction(nameof(GetMessages), new { conversationId, userId }, result);
        }
    }
}