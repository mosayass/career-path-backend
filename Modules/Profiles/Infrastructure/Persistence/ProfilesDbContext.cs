using CareerPath.Profiles.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CareerPath.Profiles.Infrastructure.Persistence
{
    public class ProfilesDbContext : DbContext
    {
        public ProfilesDbContext(DbContextOptions<ProfilesDbContext> options) : base(options) { }

        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Isolate the module's tables into its own schema
            modelBuilder.HasDefaultSchema("profiles");

            // Automatically apply UserProfileConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfilesDbContext).Assembly);
        }
    }
}