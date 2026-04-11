using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CareerPath.Profiles.Infrastructure.Persistence;

public class ProfilesDbContextFactory : IDesignTimeDbContextFactory<ProfilesDbContext>
{
    
    public ProfilesDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ProfilesDbContext>();

        // Provide a dummy connection string just for the CLI to use
        builder.UseNpgsql("Host=localhost;Port=5433;Database=CareerPathDb;Username=postgres;Password=SuperSecretPassword123!");
      
        return new ProfilesDbContext(builder.Options);
    }
}