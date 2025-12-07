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

    public DbSet<BuddyTripSummary> BuddyTripSummaries { get; set; }

    public DbSet<PendingBuddyRequest> PendingBuddyRequests { get; set; }

    public DbSet<TripDestinationInfo> TripDestinationInfo { get; set; }

    public DbSet<BuddyInfo> BuddyInfo { get; set; }

    public DbSet<BuddyRequestInfo> BuddyRequestInfo { get; set; }

    public DbSet<SimplifiedTripDestination> SimplifiedTripDestination { get; set; }
    public DbSet<TripHeaderInfo> TripHeaderInfo { get; set; }
    public DbSet<TripOverview> TripOverview { get; set; }
    public DbSet<NewTripId> NewTripIds { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Buddy>(entity =>
        {
            entity.HasKey(e => e.BuddyId).HasName("PRIMARY");

            entity.ToTable("buddies", t =>
            {
                t.HasCheckConstraint("chk_buddy_person_count", "person_count >= 1");
                t.HasCheckConstraint("chk_buddy_note_not_empty", "CHAR_LENGTH(TRIM(note)) > 0");
                t.HasCheckConstraint("chk_buddy_departure_reason_not_empty", "CHAR_LENGTH(TRIM(departure_reason)) > 0");
            });

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

            entity.ToTable("buddy_audits", t =>
            {
                t.HasCheckConstraint("chk_buddy_audit_reason_not_empty", "CHAR_LENGTH(TRIM(reason)) > 0");
            });

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

            entity.ToTable("destinations", t =>
            {
                t.HasCheckConstraint("chk_destination_name_not_empty", "CHAR_LENGTH(TRIM(name)) > 0");
                t.HasCheckConstraint("chk_destination_state_not_empty", "CHAR_LENGTH(TRIM(state)) > 0");
                t.HasCheckConstraint("chk_destination_country_not_empty", "CHAR_LENGTH(TRIM(country)) > 0");
                t.HasCheckConstraint("chk_destination_longitude", "longitude BETWEEN -180 AND 180");
                t.HasCheckConstraint("chk_destination_latitude", "latitude BETWEEN -90 AND 90");
            });

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

            entity.ToTable("trips", t =>
            {
                t.HasCheckConstraint("chk_trip_name_not_empty", "CHAR_LENGTH(TRIM(trip_name)) > 0");
                t.HasCheckConstraint("chk_trip_max_buddies", "max_buddies >= 1");
                t.HasCheckConstraint("chk_trip_description_not_empty", "CHAR_LENGTH(TRIM(description)) > 0");
                t.HasCheckConstraint("chk_trip_dates", "end_date >= start_date");
            });

            entity.HasIndex(e => e.OwnerId, "fk_trip_owner");

            entity.Property(e => e.TripId).HasColumnName("trip_id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.TripName)
                .HasMaxLength(100)
                .HasColumnName("trip_name");
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

            entity.ToTable("trip_audits", t =>
            {
                t.HasCheckConstraint("chk_trip_audit_field_not_empty", "CHAR_LENGTH(TRIM(field_changed)) > 0");
                t.HasCheckConstraint("chk_trip_audit_old_value_not_empty", "CHAR_LENGTH(TRIM(old_value)) > 0");
                t.HasCheckConstraint("chk_trip_audit_new_value_not_empty", "CHAR_LENGTH(TRIM(new_value)) > 0");
            });

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

            entity.ToTable("trip_destinations", t =>
            {
                t.HasCheckConstraint("chk_trip_destination_dates", "end_date >= start_date");
                t.HasCheckConstraint("chk_trip_destination_description_not_empty", "CHAR_LENGTH(TRIM(description)) > 0");
            });

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

            entity.ToTable("users");

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

        modelBuilder.Entity<UserAudit>().HasNoKey().Ignore(e => e.ChangedByNavigation);
        modelBuilder.Entity<User>().Ignore(t => t.UserAudits);

        modelBuilder.Entity<TripDestinationSearchResult>().HasNoKey();

        modelBuilder.Entity<BuddyTripSummary>().HasNoKey();

        modelBuilder.Entity<PendingBuddyRequest>().HasNoKey();

        modelBuilder.Entity<TripDestinationInfo>().HasNoKey();
        modelBuilder.Entity<BuddyInfo>().HasNoKey();
        modelBuilder.Entity<BuddyRequestInfo>().HasNoKey();
        modelBuilder.Entity<SimplifiedTripDestination>()
            .HasNoKey()
            .ToView("V_SimplifiedTripDest");
        modelBuilder.Entity<TripOverview>().HasNoKey();
        modelBuilder.Entity<TripHeaderInfo>().HasNoKey();

        modelBuilder.Entity<NewTripId>().HasNoKey();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
