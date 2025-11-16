using Microsoft.AspNetCore.Mvc;
using TravelBuddy.Messaging;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/conversations")]
    public class ConversationController : ControllerBase
    {
        private readonly IMessagingService _messagingService;

        public ConversationController(IMessagingService messagingService)
        {
            _messagingService = messagingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationSummaryDto>>> GetForUser(
            [FromQuery] int userId)
        {
            var result = await _messagingService.GetConversationsForUserAsync(userId);
            return Ok(result);
        }
    }
}