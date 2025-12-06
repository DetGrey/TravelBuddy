using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;
using TravelBuddy.Trips.Models;
using TravelBuddy.Messaging;
using TravelBuddy.SharedKernel;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.SharedKernel.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("api/admin")] 
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITripService _tripService;
        private readonly IMessagingService _messagingService;
        private readonly ISharedKernelService _sharedKernelService;

        public AdminController(
            IUserService userService,
            ITripService tripService,
            IMessagingService messagingService,
            ISharedKernelService sharedKernelService
        )
        {
            _userService = userService;
            _tripService = tripService;
            _messagingService = messagingService;
            _sharedKernelService = sharedKernelService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users ?? new List<UserDto>());
        }

        // TODO
        // DELETE /admin/users/{user_id}
        // Action: Permanently delete a user (use with extreme caution).
        
        // ------------------------------- AUDIT TABLES -------------------------------
        [Authorize(Roles = "admin")]
        [HttpGet("audit/users")]
        [ProducesResponseType(typeof(IEnumerable<UserAuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserAudit()
        {
            var audits = await _userService.GetUserAuditsAsync();
            return Ok(audits ?? new List<UserAuditDto>());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("audit/buddies")]
        [ProducesResponseType(typeof(IEnumerable<BuddyAuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetBuddyAudit()
        {
            var audits = await _tripService.GetBuddyAuditsAsync();
            return Ok(audits ?? new List<BuddyAuditDto>());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("audit/trips")]
        [ProducesResponseType(typeof(IEnumerable<TripAuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTripAudit()
        {
            var audits = await _tripService.GetTripAuditsAsync();
            return Ok(audits ?? new List<TripAuditDto>());
        }

        [Authorize(Roles = "admin")]
        [HttpGet("audit/conversations")]
        [ProducesResponseType(typeof(IEnumerable<ConversationAuditDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetConversationAudit()
        {
            var audits = await _messagingService.GetConversationAuditsAsync();
            return Ok(audits ?? new List<ConversationAuditDto>());
        }

        // ------------------------------- SYSTEM LOGS -------------------------------
        [Authorize(Roles = "admin")]
        [HttpGet("logs/system")]
        [ProducesResponseType(typeof(IEnumerable<SystemEventLog>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetSystemEventLogs()
        {
            var logs = await _sharedKernelService.GetSystemEventLogsAsync();
            return Ok(logs ?? new List<SystemEventLog>());
        }
    }
}