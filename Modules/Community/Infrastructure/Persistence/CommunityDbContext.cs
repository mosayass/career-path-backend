using CareerPath.Community.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Persistence;

public class CommunityDbContext(DbContextOptions<CommunityDbContext> options) : DbContext(options)
{
    public DbSet<CommunityEntity> Communities { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<DirectMessage> DirectMessages { get; set; }
    public DbSet<CommunityMember> CommunityMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Strict Schema Isolation for Modular Monolith
        modelBuilder.HasDefaultSchema("community");

        // 2. Entity Configurations
        modelBuilder.Entity<CommunityEntity>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Name).IsRequired().HasMaxLength(150);
            b.Property(c => c.Description).HasMaxLength(500);

            // Map List<string> to PostgreSQL native string array
            b.Property(c => c.MatchedCareers)
             .HasColumnType("text[]");
            // Map List<int> to PostgreSQL native integer array
            b.Property(c => c.MatchedAILabels)
             .HasColumnType("integer[]");
        });

        modelBuilder.Entity<Post>(b =>
        {
            b.HasKey(p => p.Id);
            b.Property(p => p.Title).IsRequired().HasMaxLength(250);
            b.Property(p => p.CareerTag).IsRequired().HasMaxLength(100);

            b.HasOne<CommunityEntity>()
             .WithMany()
             .HasForeignKey(p => p.CommunityId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Comment>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Body).IsRequired().HasMaxLength(2500);

            b.HasOne<Post>()
             .WithMany()
             .HasForeignKey(c => c.PostId)
             .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing relationship for Level 2+ threads
            b.HasOne<Comment>()
             .WithMany()
             .HasForeignKey(c => c.ParentCommentId)
             .OnDelete(DeleteBehavior.Restrict);// Prevent multiple cascade paths in SQL
        });

        modelBuilder.Entity<Vote>(b =>
        {
            b.HasKey(v => v.Id);

            // Enforce business rule: A user can only vote once per target (Post or Comment)
            b.HasIndex(v => new { v.UserId, v.TargetId, v.TargetType }).IsUnique();
        });
        modelBuilder.Entity<DirectMessage>(b =>
        {
            b.HasKey(m => m.Id);
            b.Property(m => m.Content).IsRequired().HasMaxLength(2000);
        });
        modelBuilder.Entity<CommunityMember>(b =>
        {
            b.HasKey(cm => cm.Id);

            // Ensure a user can only join a specific community once
            b.HasIndex(cm => new { cm.UserId, cm.CommunityId }).IsUnique();

            b.Property(cm => cm.Role).IsRequired();
            b.Property(cm => cm.IsBanned).HasDefaultValue(false);
        });
    }
}