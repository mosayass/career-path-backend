using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Infrastructure.BackgroundJobs;
using CareerPath.Assessment.Infrastructure.Clients;
using CareerPath.Assessment.Infrastructure.Persistence;
using CareerPath.Assessment.Infrastructure.Persistence.Interceptors;
using CareerPath.Assessment.Infrastructure.Repositories;
using CareerPath.Assessment.Infrastructure.Services;
using CareerPath.Shared.IntegrationEvents.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CareerPath.Assessment.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAssessmentInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind FastAPI BaseUrl from appsettings.json
        var baseUrl = configuration["AiModelService:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new System.Exception("AiModelService:BaseUrl is missing in appsettings.json");

        // Register the Typed HttpClient for communicating with the Python FastAPI service
        services.AddHttpClient<IAiModelClient, FastApiAiModelClient>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new System.Exception("DefaultConnection is missing in appsettings.json");
        
        services.AddSingleton<InsertOutboxMessagesInterceptor>();

        services.AddDbContext<AssessmentsDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<InsertOutboxMessagesInterceptor>();
            options.UseNpgsql(connectionString)
                   .AddInterceptors(interceptor);
        });

        //  Repositories & Providers 
        services.AddScoped<IAssessmentRepository, AssessmentRepository>();
        services.AddHostedService<ProcessOutboxMessagesJob>();

        services.AddScoped<IEventCollector, EventCollector>();
        return services;
    }
}