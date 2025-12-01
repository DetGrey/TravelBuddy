using System;
using System.Collections.Generic;
using TravelBuddy.Trips.Models;

namespace TravelBuddy.Messaging.Models;

public partial class Conversation
{
    public int ConversationId { get; set; }

    public int? TripDestinationId { get; set; }

    public bool IsGroup { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsArchived { get; set; }

    public virtual ICollection<ConversationAudit> ConversationAudits { get; set; } = new List<ConversationAudit>();

    public virtual ICollection<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual TripDestination? TripDestination { get; set; }
}
