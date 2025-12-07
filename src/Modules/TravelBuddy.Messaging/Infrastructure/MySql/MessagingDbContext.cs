using System;
using System.Collections.Generic;
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

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<ConversationAudit> ConversationAudits { get; set; }

    public virtual DbSet<ConversationParticipant> ConversationParticipants { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<TripDestination> TripDestinations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    // Custom
    public DbSet<ConversationOverview> ConversationOverviews { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.ConversationId).HasName("PRIMARY");

            entity.ToTable("conversations");

            entity.HasIndex(e => e.TripDestinationId, "fk_conversation_trip");

            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IsArchived).HasColumnName("is_archived");
            entity.Property(e => e.IsGroup).HasColumnName("is_group");
            entity.Property(e => e.TripDestinationId).HasColumnName("trip_destination_id");

            entity.HasOne(d => d.TripDestination).WithMany()
                .HasForeignKey(d => d.TripDestinationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_conversation_trip");
        });

        modelBuilder.Entity<ConversationAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PRIMARY");

            entity.ToTable("conversation_audits");

            entity.HasIndex(e => e.AffectedUserId, "fk_convo_audit_affected");

            entity.HasIndex(e => e.ConversationId, "fk_convo_audit_convo");

            entity.HasIndex(e => e.ChangedBy, "fk_convo_audit_changed");

            entity.Property(e => e.AuditId).HasColumnName("audit_id");
            entity.Property(e => e.Action)
                .HasColumnType("enum('created','user_added','user_removed')")
                .HasColumnName("action");
            entity.Property(e => e.AffectedUserId).HasColumnName("affected_user_id");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");

            entity.HasOne(d => d.AffectedUser).WithMany()
                .HasForeignKey(d => d.AffectedUserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_convo_audit_affected");


            entity.HasOne(d => d.ChangedByNavigation).WithMany()
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_convo_audit_changed");
        });

        modelBuilder.Entity<ConversationParticipant>(entity =>
        {
            entity.HasKey(e => new { e.ConversationId, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("conversation_participants");

            entity.HasIndex(e => e.UserId, "fk_cp_user");

            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("joined_at");

            entity.HasOne(d => d.Conversation).WithMany(p => p.ConversationParticipants)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("fk_cp_conversation");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_cp_user");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PRIMARY");

            entity.ToTable("destinations");

            entity.HasIndex(e => new { e.Country, e.State }, "idx_dest_contry_state");

            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");
            entity.Property(e => e.Latitude)
                .HasPrecision(10, 7)
                .HasColumnName("latitude");
            entity.Property(e => e.Longitude)
                .HasPrecision(10, 7)
                .HasColumnName("longitude");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.State)
                .HasMaxLength(100)
                .HasColumnName("state");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("messages", t =>
            {
                t.HasCheckConstraint("chk_message_content_not_empty", "CHAR_LENGTH(TRIM(content)) > 0");
            });

            entity.HasIndex(e => e.ConversationId, "fk_message_conversation");

            entity.HasIndex(e => e.SenderId, "fk_message_sender");

            entity.Property(e => e.MessageId).HasColumnName("message_id");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .HasColumnName("content");
            entity.Property(e => e.ConversationId).HasColumnName("conversation_id");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("sent_at");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ConversationId)
                .HasConstraintName("fk_message_conversation");

            entity.HasOne(d => d.Sender).WithMany()
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_message_sender");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.ToTable("trips");

            entity.HasIndex(e => e.OwnerId, "fk_trip_owner");

            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsArchived).HasColumnName("is_archived");
            entity.Property(e => e.MaxBuddies).HasColumnName("max_buddies");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TripName)
                .HasMaxLength(100)
                .HasColumnName("trip_name");

        });

        modelBuilder.Entity<TripDestination>(entity =>
        {
            entity.HasKey(e => e.TripDestinationId).HasName("PRIMARY");

            entity.ToTable("trip_destinations");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "idx_td_dates");

            entity.HasIndex(e => e.DestinationId, "idx_td_destination");

            entity.HasIndex(e => e.TripId, "idx_td_trip");

            entity.Property(e => e.TripDestinationId).HasColumnName("trip_destination_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.DestinationId).HasColumnName("destination_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsArchived)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_archived");
            entity.Property(e => e.SequenceNumber).HasColumnName("sequence_number");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.TripId).HasColumnName("trip_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'")
                .HasColumnType("enum('user','admin')")
                .HasColumnName("role");
        });

        modelBuilder.Entity<BuddyAudit>().HasNoKey();
        modelBuilder.Entity<TripAudit>().HasNoKey();
        modelBuilder.Entity<UserAudit>().HasNoKey().Ignore(e => e.ChangedByNavigation);

        modelBuilder.Entity<Buddy>().Ignore(b => b.BuddyAudits);
        modelBuilder.Entity<Trip>().Ignore(t => t.TripAudits);
        modelBuilder.Entity<User>().Ignore(t => t.UserAudits);


         // -------- Custom Models --------
        modelBuilder.Entity<ConversationOverview>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
