using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CareerPath.Careers.Core.Entities;

namespace CareerPath.Careers.Infrastructure.Persistence.Configurations;

public class CareerSectorConfiguration : IEntityTypeConfiguration<CareerSector>
{
    public void Configure(EntityTypeBuilder<CareerSector> builder)
    {
        builder.ToTable("CareerSectors", "careers"); // Isolated schema

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).ValueGeneratedNever(); // We are seeding specific Enum Ints

        builder.Property(s => s.Name).IsRequired().HasMaxLength(150);
    }
}

public class CareerConfiguration : IEntityTypeConfiguration<Career>
{
    public void Configure(EntityTypeBuilder<Career> builder)
    {
        builder.ToTable("Careers", "careers"); // Isolated schema

        builder.HasKey(c => c.Id);

        builder.Property(c => c.AiLabelId).IsRequired();
        builder.HasIndex(c => c.AiLabelId).IsUnique(); // Fast lookups from Assessments module

        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(1000);
        builder.Property(c => c.EducationLevel).IsRequired().HasMaxLength(150);

        // Map the string list to a PostgreSQL JSONB column natively
        builder.Property(c => c.CoreSkills).HasColumnType("jsonb");

        builder.HasOne(c => c.Sector)
               .WithMany(s => s.Careers)
               .HasForeignKey(c => c.SectorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}