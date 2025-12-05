using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TravelBuddy.Api.Auth;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;
using TravelBuddy.Trips.DTOs;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    // [Route] defines the base URL for this controller
    [Route("api/users/{userId}/buddy-requests")]
    public class BuddyRequestsController : ControllerBase
    {
        private readonly ITripService _tripService;

        public BuddyRequestsController(
            ITripService tripService
        )
        {
            _tripService = tripService;
        }

        [Authorize]
        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostBuddyRequest([FromRoute] int userId, [FromBody] BuddyDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            dto.UserId = userId;

            // TODO Ideally, your Service should return the created object (with its new ID)
            // assuming InsertBuddyRequestAsync returns (bool success, string error, BuddyDto createdBuddy)
            var (success, errorMessage) = await _tripService.InsertBuddyRequestAsync(dto);
            
            if (!success) return BadRequest(errorMessage ?? "Buddy request failed");

            // Standard: Return the created resource.
            return Created();
        }

        [Authorize]
        [HttpGet("pending")]
        [ProducesResponseType(typeof(IEnumerable<PendingBuddyRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingBuddyRequests([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var pendingBuddyRequests = await _tripService.GetPendingBuddyRequestsAsync(userId);
            if (!pendingBuddyRequests.Any()) return NoContent();

            return Ok(pendingBuddyRequests ?? new List<PendingBuddyRequestDto>());
        }

        [Authorize]
        [HttpPatch()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateBuddyRequest(
            [FromRoute] int userId,
            [FromBody] UpdateBuddyRequestDto updateBuddyRequestDto
        ) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            updateBuddyRequestDto.UserId = userId;

            if (!User.IsSelfOrAdmin(updateBuddyRequestDto.UserId)) return Forbid();

            var (success, errorMessage) = await _tripService.UpdateBuddyRequestAsync(updateBuddyRequestDto);
            if (!success) return BadRequest(new { error = errorMessage ?? "Updating buddy request status failed" });

            return NoContent();
        }
    }
}