using CareerPath.Assessment.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerPath.Assessment.Infrastructure.Persistence.Configurations;

public class AssessmentResultConfiguration : IEntityTypeConfiguration<AssessmentResult>
{
    public void Configure(EntityTypeBuilder<AssessmentResult> builder)
    {
        // 1. Schema Isolation
        builder.ToTable("AssessmentResults", "assessments");

        // 2. Primary Key
        builder.HasKey(r => r.Id);

        // 3. String Limits
        builder.Property(r => r.PrimaryCareer)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(r => r.SecondaryCareer)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(r => r.TertiaryCareer)
            .HasMaxLength(255)
            .IsRequired();

        // 4. Decimal Precision (e.g., 0.9543)
        builder.Property(r => r.PrimaryConfidence)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.SecondaryConfidence)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.TertiaryConfidence)
            .HasColumnType("decimal(5,4)")
            .IsRequired();

        builder.Property(r => r.GeneratedAt)
            .IsRequired();
    }
}