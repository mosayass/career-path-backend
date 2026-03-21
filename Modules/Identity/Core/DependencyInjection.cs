using CareerPath.Shared.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CareerPath.Identity.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddIdentityCoreServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR and our custom Validation Pipeline Behavior
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register all FluentValidation rules in this project
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}