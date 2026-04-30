using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Infrastructure.Persistence;
using CareerPath.Community.Infrastructure.Queries;
using CareerPath.Community.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
// using CareerPath.Shared.Interceptors; // Uncomment if you are using InsertOutboxMessagesInterceptor

namespace CareerPath.Community.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddCommunityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Configure DbContext
        services.AddDbContext<CommunityDbContext>((sp, options) =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            // If you use the Outbox Interceptor globally, resolve and attach it
            // var interceptor = sp.GetService<InsertOutboxMessagesInterceptor>();
            // if (interceptor != null) options.AddInterceptors(interceptor);
        });

        // 2. Register Repositories (Write Models)
        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<ICommunityMemberRepository, CommunityMemberRepository>();
        services.AddScoped<IPostRepository, PostRepository>(); // Assuming you have this
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();

        // 3. Register Queries (Read Models)
        services.AddScoped<ICommunityFeedQueries, CommunityFeedQueries>();
        services.AddScoped<ICommunityDiscoveryQueries, CommunityDiscoveryQueries>();

        // 4. Register the Seeder so the Host can resolve it
        services.AddScoped<CommunityDataSeeder>();

        return services;
    }
}