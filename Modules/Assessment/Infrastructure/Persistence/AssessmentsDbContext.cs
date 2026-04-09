using CareerPath.Assessment.Core.Entities;
using CareerPath.Shared.IntegrationEvents.Outbox;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Assessment.Infrastructure.Persistence;

public class AssessmentsDbContext : DbContext
{
    public AssessmentsDbContext(DbContextOptions<AssessmentsDbContext> options)
        : base(options)
    {
    }

    public DbSet<AssessmentSubmission> AssessmentSubmissions { get; set; } = null!;
    public DbSet<AssessmentResult> AssessmentResults { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sets the default schema for any tables that might not have it explicitly defined
        modelBuilder.HasDefaultSchema("assessments");

        // This automatically finds and applies the Fluent API configurations 
        // (AssessmentSubmissionConfiguration and AssessmentResultConfiguration) 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssessmentsDbContext).Assembly);

        modelBuilder.Entity<OutboxMessage>(builder =>
        {
            builder.ToTable("OutboxMessages");
            builder.HasKey(x => x.Id);
        });
    }
}