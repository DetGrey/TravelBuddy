using Microsoft.EntityFrameworkCore;
using TravelBuddy.Users.Models;

namespace TravelBuddy.Users.Infrastructure;

public partial class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAudit> UserAudits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

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

        modelBuilder.Entity<UserAudit>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PRIMARY");

            entity.ToTable("user_audit");
            entity.HasIndex(e => e.UserId, "fk_user_audit_user");

            entity.HasIndex(e => e.ChangedBy, "fk_user_audit_changed_by");

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
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.Ignore(e => e.ChangedByNavigation);

            entity.HasOne(d => d.User).WithMany(p => p.UserAudits)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_user_audit_user");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
