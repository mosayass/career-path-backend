using CareerPath.Careers.Core.Contracts;
using CareerPath.Careers.Infrastructure.Persistence;
using CareerPath.Careers.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CareerPath.Careers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCareersInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CareersDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        // Register the seeder so the Host can resolve it
        services.AddScoped<CareersDataSeeder>();
        // Add this inside AddCareersInfrastructure
        services.AddScoped<ICareerRepository, CareerRepository>();
        return services;
    }
}