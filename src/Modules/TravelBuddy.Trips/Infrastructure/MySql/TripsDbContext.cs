using Microsoft.EntityFrameworkCore;
using TravelBuddy.Trips.DTOs;
using TravelBuddy.Trips.Models;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Trips.Infrastructure;

public partial class TripsDbContext : DbContext
{
    public TripsDbContext(DbContextOptions<TripsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buddy> Buddies { get; set; }

    public virtual DbSet<BuddyAudit> BuddyAudits { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<TripAudit> TripAudits { get; set; }

    public virtual DbSet<TripDestination> TripDestinations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<TripDestinationSearchResult> TripDestinationSearchResults { get; set; }

    public DbSet<UserTripSummary> UserTripSummaries { get; set; }

    public DbSet<PendingBuddyRequest> PendingBuddyRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Buddy>(entity =>
        {
            entity.HasKey(e => e.BuddyId).HasName("PRIMARY");

            entity.ToTable("buddy");

            entity.HasIndex(e => e.UserId, "fk_buddy_user");

            entity.HasIndex(e => new { e.TripDestinationId, e.RequestStatus }, "idx_buddy_seg_status");

            entity.Property(e => e.BuddyId).HasColumnName("buddy_id");
            entity.Property(e => e.DepartureReason)
                .HasMaxLength(255)
                .HasColumnName("departure_reason");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .HasColumnName("note");
            entity.Property(e => e.PersonCount)
                .HasDefaultValueSql("'1'")
                .HasColumnName("person_count");
            entity.Property(e => e.RequestStatus)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','accepted','rejected')")
                .HasColumnName("request_status");
            entity.Property(e => e.TripDestinationId).HasColumnName("trip_destination_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.TripDestination).WithMany(p => p.Buddies)
                .HasForeignKey(d => d.TripDestinationId)
                .HasConstraintName("fk_buddy_tripDestination");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_buddy_user");
        });

        modelBuilder.Entity<BuddyAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PRIMARY");

            entity.ToTable("buddy_audit");

            entity.HasIndex(e => e.BuddyId, "fk_buddy_audit_buddy");

            entity.HasIndex(e => e.ChangedBy, "fk_buddy_audit_user");

            entity.Property(e => e.AuditId).HasColumnName("audit_id");
            entity.Property(e => e.Action)
                .HasColumnType("enum('requested','accepted','rejected','removed','left')")
                .HasColumnName("action");
            entity.Property(e => e.BuddyId).HasColumnName("buddy_id");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.Reason)
                .HasMaxLength(255)
                .HasColumnName("reason");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.Buddy).WithMany(p => p.BuddyAudits)
                .HasForeignKey(d => d.BuddyId)
                .HasConstraintName("fk_buddy_audit_buddy");

            entity.HasOne(d => d.ChangedByNavigation).WithMany()
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_buddy_audit_user");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("PRIMARY");

            entity.ToTable("destination");

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

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.ToTable("trip");

            entity.HasIndex(e => e.OwnerId, "fk_trip_owner");

            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsArchived)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_archived");
            entity.Property(e => e.MaxBuddies).HasColumnName("max_buddies");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Owner).WithMany()
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_trip_owner");
        });

        modelBuilder.Entity<TripAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PRIMARY");

            entity.ToTable("trip_audit");

            entity.HasIndex(e => e.TripId, "fk_trip_audit_trip");

            entity.HasIndex(e => e.ChangedBy, "fk_trip_audit_user");

            entity.Property(e => e.AuditId).HasColumnName("audit_id");
            entity.Property(e => e.Action)
                .HasColumnType("enum('created','updated','deleted')")
                .HasColumnName("action");
            entity.Property(e => e.ChangedBy).HasColumnName("changed_by");
            entity.Property(e => e.FieldChanged)
                .HasMaxLength(100)
                .HasColumnName("field_changed");
            entity.Property(e => e.NewValue)
                .HasMaxLength(255)
                .HasColumnName("new_value");
            entity.Property(e => e.OldValue)
                .HasMaxLength(255)
                .HasColumnName("old_value");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
            entity.Property(e => e.TripId).HasColumnName("trip_id");

            entity.HasOne(d => d.ChangedByNavigation).WithMany()
                .HasForeignKey(d => d.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_trip_audit_user");

            entity.HasOne(d => d.Trip).WithMany(p => p.TripAudits)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("fk_trip_audit_trip");
        });

        modelBuilder.Entity<TripDestination>(entity =>
        {
            entity.HasKey(e => e.TripDestinationId).HasName("PRIMARY");

            entity.ToTable("trip_destination");

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

            entity.HasOne(d => d.Destination).WithMany(p => p.TripDestinations)
                .HasForeignKey(d => d.DestinationId)
                .HasConstraintName("fk_itinerary_destination");

            entity.HasOne(d => d.Trip).WithMany(p => p.TripDestinations)
                .HasForeignKey(d => d.TripId)
                .HasConstraintName("fk_itinerary_trip");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Birthdate).HasColumnName("birthdate");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("'0'")
                .HasColumnName("is_deleted");
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

        modelBuilder.Entity<TripDestinationSearchResult>().HasNoKey();

        modelBuilder.Entity<UserTripSummary>().HasNoKey();

        modelBuilder.Entity<PendingBuddyRequest>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
