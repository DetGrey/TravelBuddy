using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TravelBuddy.Api.Auth;
using TravelBuddy.Users;
using TravelBuddy.Users.DTOs;
using TravelBuddy.Trips;

namespace TravelBuddy.Api.Controllers
{
    // [ApiController] enables automatic API features like model validation.
    [ApiController]
    // [Route] defines the base URL for this controller (e.g., /api/users).
    [Route("api/users")] 
    public class UsersController : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        // This is the dependency on the business logic layer (UserService).
        private readonly IUserService _userService;
        private readonly ITripService _tripService;    


        // Constructor: ASP.NET Core automatically injects the IUserService implementation here.
        public UsersController(
            JwtTokenGenerator jwtTokenGenerator,
            IUserService userService,
            ITripService tripDestinationService
        )
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userService = userService;
            _tripService = tripDestinationService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) return Unauthorized("Invalid credentials");

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return StatusCode(StatusCodes.Status500InternalServerError, "Token generation failed.");
            
            var userDto = new UserDto(
                user.UserId,
                user.Name,
                user.Email,
                user.Birthdate
            );

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Ok(new AuthResponseDto(userDto, token));
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.RegisterAsync(request);
            if (user == null) return Conflict("Email already in use");

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return StatusCode(StatusCodes.Status500InternalServerError, "Token generation failed.");

            var userDto = new UserDto(
                user.UserId,
                user.Name,
                user.Email,
                user.Birthdate
            );

            Response.Cookies.Append("access_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1)
            });

            return Created($"/api/users/{userDto.UserId}", new AuthResponseDto(userDto, token));
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<string> Logout()
        {
            Response.Cookies.Delete("access_token");
            return Ok("Logged out successfully.");
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId))
                return Forbid();
        
            var user = await _userService.GetUserByIdAsync(userId);
            
            if (user == null)
            {
                return NoContent();
            }

            return Ok(user);
        }
        
        [Authorize]
        [HttpDelete("{userId}/delete-user")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId))
                return Forbid();

            var success = await _userService.DeleteUser(userId);
            if (!success)
                 return BadRequest("User deletion failed due to invalid input or policy violation.");

            if (!User.IsAdmin(userId))
                Response.Cookies.Delete("access_token");

            return Ok("User deleted successfully.");
        }

        [Authorize]
        [HttpPost("{userId}/change-password")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword([FromRoute] int userId, [FromBody] PasswordChangeRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Note: this means that admin can change other people's passwords
            // This is not ideal for a real project but used here to access our generated data
            if (!User.IsSelfOrAdmin(userId))
                return Forbid();

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null)
                return NotFound("User email not found in claims.");

            var success = await _userService.ChangePasswordAsync(request, userEmail, userId);
            if (!success)
                 return BadRequest("Password change failed due to invalid input or policy violation.");

            return Ok("Password changed successfully.");
        }
    }
}