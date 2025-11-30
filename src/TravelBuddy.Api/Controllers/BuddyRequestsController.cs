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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> PostBuddyRequest([FromRoute] int userId, [FromBody] BuddyDto dto)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            dto.UserId = userId;

            var (success, errorMessage) = await _tripService.InsertBuddyRequestAsync(dto);
            
            if (!success) return BadRequest(errorMessage ?? "Buddy request failed");

            return Created();
        }

        [Authorize]
        [HttpGet("pending")]
        [ProducesResponseType(typeof(IEnumerable<PendingBuddyRequestDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingBuddyRequests([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var pendingBuddyRequests = await _tripService.GetPendingBuddyRequestsAsync(userId);
            if (!pendingBuddyRequests.Any()) return NoContent();

            return Ok(pendingBuddyRequests);
        }

        [Authorize]
        [HttpPost("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateBuddyRequest(
            [FromRoute] int userId,
            [FromBody] UpdateBuddyRequestDto updateBuddyRequestDto
        ) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            updateBuddyRequestDto.UserId = userId;

            if (!User.IsSelfOrAdmin(updateBuddyRequestDto.UserId)) return Forbid();

            var (success, errorMessage) = await _tripService.UpdateBuddyRequestAsync(updateBuddyRequestDto);
            if (!success) return BadRequest(errorMessage ?? "Updating buddy request status failed");

            return Ok($"Buddy with buddy id {updateBuddyRequestDto.BuddyId} has been {updateBuddyRequestDto.NewStatus}");
        }
    }
}