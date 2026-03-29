using Microsoft.EntityFrameworkCore;
using CareerPath.Careers.Core.Entities;

namespace CareerPath.Careers.Infrastructure.Persistence;

public class CareersDbContext : DbContext
{
    public CareersDbContext(DbContextOptions<CareersDbContext> options) : base(options) { }

    public DbSet<CareerSector> CareerSectors => Set<CareerSector>();
    public DbSet<Career> Careers => Set<Career>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("careers");

        // Automatically applies CareerSectorConfiguration and CareerConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CareersDbContext).Assembly);
    }
}