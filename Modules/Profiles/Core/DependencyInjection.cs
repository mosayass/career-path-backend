using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CareerPath.Profiles.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProfilesCore(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            // This will automatically register our Commands, Queries, AND our Integration Event Handlers
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(assembly));

            // Registers UpdateProfileCommandValidator and GetProfileByIdQueryValidator
            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}