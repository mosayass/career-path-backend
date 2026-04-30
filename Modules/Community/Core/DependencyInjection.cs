using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using CareerPath.Shared.Behaviors;

namespace CareerPath.Community.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCommunityCore(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR and the custom Validation Pipeline Behavior
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all FluentValidation rules in the Community Core project
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}