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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.AuthenticateAsync(request.Email, request.Password);
            if (user == null) return Problem("Invalid credentials", statusCode: StatusCodes.Status401Unauthorized);

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return Problem("Token generation failed.", statusCode: StatusCodes.Status500InternalServerError);
            
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
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.RegisterAsync(request);
            if (user == null) return Problem("Email already in use", statusCode: StatusCodes.Status409Conflict);

            var token = _jwtTokenGenerator.GenerateToken(user);
            if (string.IsNullOrEmpty(token))
                return Problem("Token generation failed.", statusCode: StatusCodes.Status500InternalServerError);
            
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

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("access_token");
            return NoContent();
        }

        [Authorize]
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();
        
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound();

            return Ok(user);
        }
        
        [Authorize]
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var (success, errorMessage) = await _userService.DeleteUser(userId);
            if (!success) return BadRequest(new { error = errorMessage });

            if (!User.IsAdmin(userId))
                Response.Cookies.Delete("access_token");

            return NoContent();
        }

        [Authorize]
        [HttpPatch("{userId}/change-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePassword([FromRoute] int userId, [FromBody] PasswordChangeRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!User.IsSelfOrAdmin(userId)) return Forbid();

            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (userEmail == null) return Problem("Claims error", statusCode: StatusCodes.Status404NotFound);

            var (success, errorMessage) = await _userService.ChangePasswordAsync(request, userEmail, userId);
            if (!success) return BadRequest(new { error = errorMessage });

            return NoContent();
        }
    }
}