using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CareerPath.Assessment.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddAssessmentCore(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        // Register MediatR for all Commands/Queries in this assembly
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));

        // Register all FluentValidation validators (like our SubmitAssessmentCommandValidator)
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}