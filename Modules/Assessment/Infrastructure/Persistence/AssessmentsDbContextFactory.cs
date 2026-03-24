using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CareerPath.Assessment.Infrastructure.Persistence;

public class AssessmentsDbContextFactory : IDesignTimeDbContextFactory<AssessmentsDbContext>
{
    public AssessmentsDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<AssessmentsDbContext>();

        // Using the proven hardcoded connection string for local CLI migrations
        builder.UseNpgsql("Host=localhost;Port=5433;Database=CareerPathDb;Username=postgres;Password=SuperSecretPassword123!");

        return new AssessmentsDbContext(builder.Options);
    }
}