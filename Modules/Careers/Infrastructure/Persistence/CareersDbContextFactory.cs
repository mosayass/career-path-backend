using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CareerPath.Careers.Infrastructure.Persistence;

public class CareersDbContextFactory : IDesignTimeDbContextFactory<CareersDbContext>
{
    public CareersDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<CareersDbContext>();

        // Using your exact CLI connection string
        builder.UseNpgsql("Host=localhost;Port=5433;Database=CareerPathDb;Username=postgres;Password=SuperSecretPassword123!");

        return new CareersDbContext(builder.Options);
    }
}