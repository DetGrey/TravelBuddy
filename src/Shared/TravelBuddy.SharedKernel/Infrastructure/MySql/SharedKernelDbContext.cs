using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TravelBuddy.SharedKernel.Models;

namespace TravelBuddy.SharedKernel.Infrastructure;

public partial class SharedKernelDbContext : DbContext
{
    public SharedKernelDbContext(DbContextOptions<SharedKernelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SystemEventLog> SystemEventLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<SystemEventLog>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");

            entity.ToTable("system_event_logs");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.AffectedId).HasColumnName("affected_id");
            entity.Property(e => e.Details)
                .HasMaxLength(255)
                .HasColumnName("details");
            entity.Property(e => e.EventType)
                .HasMaxLength(100)
                .HasColumnName("event_type");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("timestamp");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
