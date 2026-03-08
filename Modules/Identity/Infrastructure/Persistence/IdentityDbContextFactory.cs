using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CareerPath.Identity.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        // Tell the factory to look inside the Host project folder for the configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Host")))
            .AddJsonFile("appsettings.Development.json", optional: false)
            .Build();

        var builder = new DbContextOptionsBuilder<IdentityDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseNpgsql(connectionString);

        return new IdentityDbContext(builder.Options);
    }
}