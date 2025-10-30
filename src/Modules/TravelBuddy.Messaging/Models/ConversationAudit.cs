using System;
using System.Collections.Generic;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging.Models;

public partial class ConversationAudit
{
    public int AuditId { get; set; }

    public int ConversationId { get; set; }

    public int? AffectedUserId { get; set; }

    public string Action { get; set; } = null!;

    public int? TriggeredBy { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User? AffectedUser { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User? TriggeredByNavigation { get; set; }
}
