using TravelBuddy.Users.Models;

namespace TravelBuddy.Trips.Models;

public partial class BuddyAudit
{
    public int AuditId { get; set; }

    public int BuddyId { get; set; }

    public string Action { get; set; } = null!;

    public string? Reason { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Buddy Buddy { get; set; } = null!;

    public virtual User? ChangedByNavigation { get; set; }
}
