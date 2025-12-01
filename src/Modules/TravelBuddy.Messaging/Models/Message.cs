using System;
using System.Collections.Generic;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int? SenderId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }

    public int ConversationId { get; set; }

    public virtual Conversation Conversation { get; set; } = null!;

    public virtual User? Sender { get; set; }
}
