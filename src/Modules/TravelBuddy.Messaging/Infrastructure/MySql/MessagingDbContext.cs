using Microsoft.EntityFrameworkCore;
using TravelBuddy.Messaging.Models;
using TravelBuddy.Trips.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Messaging.Infrastructure;

public partial class MessagingDbContext : DbContext
{
    public MessagingDbContext(DbContextOptions<MessagingDbContext> options)
        : base(options)
    {
    }

    // Messaging tables only
    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<ConversationParticipant> ConversationParticipants { get; set; }
    public DbSet<Message> Messages { get; set; }

    // Needed for sender_id and user_id
    public DbSet<User> Users { get; set; }

    // Custom
    public DbSet<ConversationOverview> ConversationOverviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Conversation ---
        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId);

            entity.ToTable("conversation");

            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.IsArchived).HasColumnName("is_archived");
            entity.Property(e => e.IsGroup).HasColumnName("is_group");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.TripDestinationId).HasColumnName("trip_destination_id");
        });

        // --- Conversation Participant ---
        modelBuilder.Entity<ConversationParticipant>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.UserId });

            entity.ToTable("conversation_participant");

            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.JoinedAt).HasColumnName("joined_at");
        });

        // --- Message ---
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId);

            entity.ToTable("message");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.SentAt).HasColumnName("sent_at");
        });

        // --- User ---
        modelBuilder.Entity<User>(entity =>
        {
        entity.HasKey(e => e.UserId).HasName("PRIMARY");

        entity.ToTable("user");

        entity.HasIndex(e => e.Email, "email").IsUnique();

        entity.Property(e => e.UserId).HasColumnName("user_id");
        entity.Property(e => e.Birthdate).HasColumnName("birthdate");

        entity.Property(e => e.PasswordHash).HasColumnName("password_hash");

        entity.Property(e => e.Email)
            .HasMaxLength(150)
            .HasColumnName("email");
        entity.Property(e => e.IsDeleted)
            .HasDefaultValueSql("'0'")
            .HasColumnName("is_deleted");
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .HasColumnName("name");
   
        });
        
        // -------- AUDIT & NAVIGATION FIX --------

        // Markér BuddyAudit & TripAudit som keyless (de har ingen PK i databasen)
        modelBuilder.Entity<BuddyAudit>().HasNoKey();
        modelBuilder.Entity<TripAudit>().HasNoKey();

        // Ignorér navigationer der peger på audits
        modelBuilder.Entity<Buddy>().Ignore(b => b.BuddyAudits);
        modelBuilder.Entity<Trip>().Ignore(t => t.TripAudits);

        // Vi bruger ikke ConversationAudit i Messaging
        modelBuilder.Ignore<ConversationAudit>();
        modelBuilder.Entity<Conversation>().Ignore(c => c.ConversationAudits);

        // -------- Custom Models --------
        modelBuilder.Entity<ConversationOverview>().HasNoKey();
    }

}
