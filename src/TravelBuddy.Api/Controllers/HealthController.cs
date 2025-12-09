using Microsoft.AspNetCore.Mvc;

namespace TravelBuddy.Api.Controllers
{
    [ApiController]
    [Route("")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Root()
        {
            var request = HttpContext.Request;
            var swaggerLink = $"{request.Scheme}://{request.Host}/swagger";
            
            return Ok(new
            {
                message = "Welcome to TravelBuddy",
                swaggerUrl = swaggerLink,
                timestamp = DateTime.UtcNow
            });
        }

        [HttpGet]
        [Route("api/health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}
