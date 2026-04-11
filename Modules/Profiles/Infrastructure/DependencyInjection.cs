using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CareerPath.Profiles.Core.Contracts;
// Ensure these namespaces match your exact folder structure
using CareerPath.Profiles.Infrastructure.Persistence;
using CareerPath.Profiles.Infrastructure.Repositories;

namespace CareerPath.Profiles.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProfilesInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new System.Exception("DefaultConnection is missing in appsettings.json");

            // ProfilesDbContext does not need the Outbox interceptor yet since it only consumes events
            services.AddDbContext<ProfilesDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            // Register the User Profile Repository
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            return services;
        }
    }
}