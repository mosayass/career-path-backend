using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Infrastructure.Persistence
{
    public class CommunityDbContextFactory : IDesignTimeDbContextFactory<CommunityDbContext>
    {

        public CommunityDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CommunityDbContext>();

            // Provide a dummy connection string just for the CLI to use
            builder.UseNpgsql("Host=localhost;Port=5433;Database=CareerPathDb;Username=postgres;Password=SuperSecretPassword123!");

            return new CommunityDbContext(builder.Options);
        }
    }
}
