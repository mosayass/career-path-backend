using System.Reflection;
using CareerPath.Shared.Behaviors; // Ensure this matches where your ValidationBehavior lives
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CareerPath.Careers.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCareersCore(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}