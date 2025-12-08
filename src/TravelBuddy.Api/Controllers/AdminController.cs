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
using TravelBuddy.Api.Auth;

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

        [Authorize(Roles = "admin")]
        [HttpDelete("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _userService.AdminDeleteUserAsync(userId, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "User Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Delete Failed",
                    Detail = errorMessage ?? "Failed to delete user.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpPatch("users/{userId}/role")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleRequest request)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _userService.UpdateUserRoleAsync(userId, request.Role, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true || errorMessage?.Contains("Target user") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "User Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Update Failed",
                    Detail = errorMessage ?? "Failed to update user role.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

        // ------------------------------- TRIP DELETION -------------------------------
        [Authorize(Roles = "admin")]
        [HttpDelete("trips/{tripId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTrip(int tripId)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _tripService.DeleteTripAsync(tripId, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Trip Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Delete Failed",
                    Detail = errorMessage ?? "Failed to delete trip.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("trip-destinations/{tripDestinationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTripDestination(int tripDestinationId)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _tripService.DeleteTripDestinationAsync(tripDestinationId, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Trip Destination Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Delete Failed",
                    Detail = errorMessage ?? "Failed to delete trip destination.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("destinations/{destinationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDestination(int destinationId)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _tripService.DeleteDestinationAsync(destinationId, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Destination Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Delete Failed",
                    Detail = errorMessage ?? "Failed to delete destination.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("conversations/{conversationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteConversation(int conversationId)
        {
            int? adminUserId = User.GetUserId();
            if (adminUserId == null)
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Unauthorized",
                    Detail = "User is not authenticated.",
                    Status = StatusCodes.Status401Unauthorized
                });
            }

            var (success, errorMessage) = await _messagingService.DeleteConversationAsync(conversationId, adminUserId.Value);
            
            if (!success)
            {
                if (errorMessage?.Contains("not found") == true)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Conversation Not Found",
                        Detail = errorMessage,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return BadRequest(new ProblemDetails
                {
                    Title = "Delete Failed",
                    Detail = errorMessage ?? "Failed to delete conversation.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            return NoContent();
        }

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