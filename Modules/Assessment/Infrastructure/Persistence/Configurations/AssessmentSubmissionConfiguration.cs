using CareerPath.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPath.Assessment.Infrastructure.Persistence.Configurations;

public class AssessmentSubmissionConfiguration : IEntityTypeConfiguration<AssessmentSubmission>
{
    public void Configure(EntityTypeBuilder<AssessmentSubmission> builder)
    {
        // 1. Schema Isolation
        builder.ToTable("AssessmentSubmissions", "assessments");

        // 2. Primary Key
        builder.HasKey(s => s.Id);

        // We index the UserId because we will frequently query "Get all assessments for this user"
        builder.HasIndex(s => s.UserId);

        // 3. PostgreSQL Native JSONB
        builder.Property(s => s.RawAnswersJson)
            .HasColumnType("jsonb")
            .IsRequired();

        builder.Property(s => s.SubmittedAt)
            .IsRequired();

        // 4. One-to-One Relationship mapping
        builder.HasOne(s => s.Result)
            .WithOne(r => r.Submission)
            .HasForeignKey<AssessmentResult>(r => r.AssessmentSubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}