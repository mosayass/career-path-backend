using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CareerPath.Identity.Infrastructure.Persistence;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    // Old way to migration before adding logic but now it gives
    // error so I will replace it with the new and I hope it work 21/03/2026

    //public IdentityDbContext CreateDbContext(string[] args)
    //{
    //    // Tell the factory to look inside the Host project folder for the configuration
    //    var configuration = new ConfigurationBuilder()
    //        .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Host")))
    //        .AddJsonFile("appsettings.Development.json", optional: false)
    //        .Build();

    //    var builder = new DbContextOptionsBuilder<IdentityDbContext>();
    //    var connectionString = configuration.GetConnectionString("DefaultConnection");

    //    builder.UseNpgsql(connectionString);

    //    return new IdentityDbContext(builder.Options);
    //}
    public IdentityDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<IdentityDbContext>();

        // Provide a dummy connection string just for the CLI to use
        builder.UseNpgsql("Host=localhost;Port=5433;Database=CareerPathDb;Username=postgres;Password=SuperSecretPassword123!");
      
        return new IdentityDbContext(builder.Options);
    }
}