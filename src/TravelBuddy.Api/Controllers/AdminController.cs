using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;
using TravelBuddy.Trips.Models;
using TravelBuddy.Messaging;
using TravelBuddy.SharedKernel;

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
        [HttpGet("admin-action")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult AdminOnlyStuff() => Ok("Admins only!");

        [Authorize(Roles = "admin")]
        [HttpGet("users")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users == null || !users.Any())
                return NoContent();

            return Ok(users);
        }

        // TODO
        // DELETE /admin/users/{user_id}
        // Action: Permanently delete a user (use with extreme caution).
        
        // ------------------------------- AUDIT TABLES -------------------------------
        [Authorize(Roles = "admin")]
        [HttpGet("audit/buddy")]
        [ProducesResponseType(typeof(IEnumerable<BuddyAudit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetBuddyAudit()
        {
            var audits = _tripService.GetBuddyAuditsAsync();

            if (audits == null || !audits.Result.Any())
                return NoContent();
            
            return Ok(audits.Result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("audit/trip")]
        [ProducesResponseType(typeof(IEnumerable<TripAudit>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetTripAudit()
        {
            var audits = _tripService.GetTripAuditsAsync();

            if (audits == null || !audits.Result.Any())
                return NoContent();

            return Ok(audits.Result);
        }

        [Authorize(Roles = "admin")]
        [HttpGet("audit/conversation")]
        public IActionResult GetConversationAudit()
        {
            var audits = _messagingService.GetConversationAuditsAsync();

            if (audits == null || !audits.Result.Any())
                return NoContent();

            return Ok(audits.Result);
        }

        // ------------------------------- SYSTEM LOGS -------------------------------
        [Authorize(Roles = "admin")]
        [HttpGet("logs/system")]
        public IActionResult GetSystemEventLogs()
        {
            var logs = _sharedKernelService.GetSystemEventLogsAsync();
            if (logs == null || !logs.Result.Any())
                return NoContent();

            return Ok(logs.Result);
        }
    }
}