using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using CareerPath.Assessment.Core.Contracts;
using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Infrastructure.Services;

public class AppSettingsCareerMappingProvider : ICareerMappingProvider
{
    private readonly Dictionary<int, string> _mappings;

    public AppSettingsCareerMappingProvider(IConfiguration configuration)
    {
        // Loads the AI labels directly from appsettings.json
        _mappings = configuration.GetSection("AiCareerMappings").Get<Dictionary<int, string>>()
                    ?? new Dictionary<int, string>();
    }

    public Result<string> GetCareerName(int jobLabel)
    {
        if (_mappings.TryGetValue(jobLabel, out var careerName))
        {
            return Result<string>.Success(careerName);
        }

        return Result<string>.Failure($"CRITICAL: No mapped career name found for AI job label '{jobLabel}'. Please update appsettings.json.");
    }
}