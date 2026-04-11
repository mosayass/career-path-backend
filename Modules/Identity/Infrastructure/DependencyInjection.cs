using CareerPath.Identity.Core.Contracts;
using CareerPath.Identity.Core.Entities;
using CareerPath.Identity.Infrastructure.Persistence;
using CareerPath.Identity.Infrastructure.Repositories;
using CareerPath.Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CareerPath.Shared.IntegrationEvents.Contracts;
using CareerPath.Shared.Infrastructure.Outbox;

namespace CareerPath.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddDbContext<IdentityDbContext>((sp, options) =>
        {
            // Resolve the interceptor from the DI container
            var interceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();

            // 2. Configure Postgres and attach the interceptor
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                   .AddInterceptors(interceptor);
        });

        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.User.RequireUniqueEmail = true;
        })
        .AddRoles<Role>()
        .AddEntityFrameworkStores<IdentityDbContext>();

        // 2. JWT & INTERFACE ADAPTERS (New Code)
        

        // Bind JWT Options from appsettings.json
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        // Register Contracts & Adapters
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtProvider, JwtProvider>();

        // Configure JWT Authentication Middleware
        var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();
        if (jwtOptions == null) throw new System.Exception("JwtOptions is missing in appsettings.json");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey))
            };
        });


        services.AddScoped<IEmailService, MailtrapEmailService>();


        //  Register the Shared Interceptor as a Singleton
        services.AddSingleton<InsertOutboxMessagesInterceptor>();

        // 2. Register the Mailbag as Scoped (one per HTTP request)
        services.AddScoped<IEventCollector, EventCollector>();

        // 3. Register the Background Worker explicitly targeting the Identity DB
        services.AddHostedService<ProcessOutboxMessagesJob<IdentityDbContext>>();

        return services;
    }
}