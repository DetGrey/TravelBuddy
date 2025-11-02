using System.Security.Claims;

namespace TravelBuddy.Api.Auth
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsSelfOrAdmin(this ClaimsPrincipal user, int targetUserId)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

            if (userIdClaim == null || roleClaim == null)
                return false; // JWT is malformed or missing required claims

            var isAdmin = roleClaim.Equals("admin", StringComparison.OrdinalIgnoreCase);
            var isOwner = int.TryParse(userIdClaim, out var userId) && userId == targetUserId;

            return isAdmin || isOwner; // Not allowed to access another user's data unless you're admin
        }
    }
}