using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CareerPath.Community.Core.Entities;

namespace CareerPath.Community.Infrastructure.Persistence;

public class CommunityDataSeeder
{
    private readonly CommunityDbContext _context;
    private readonly ILogger<CommunityDataSeeder> _logger;

    public CommunityDataSeeder(CommunityDbContext context, ILogger<CommunityDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (_context.Set<CommunityEntity>().Any())
            {
                _logger.LogInformation("Community data is already seeded.");
                return;
            }

            var filePath = Path.Combine(AppContext.BaseDirectory, "SeedData", "CommunitiesSeed.json");

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Seed file not found at path: {FilePath}", filePath);
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var seedData = JsonSerializer.Deserialize<List<CommunitySeedDto>>(jsonData, options);

            if (seedData != null && seedData.Any())
            {
                var entities = new List<CommunityEntity>();

                foreach (var dto in seedData)
                {
                    var entity = new CommunityEntity(
                        dto.Name,
                        dto.Description,
                        dto.MatchedCareers,
                        dto.MatchedAILabels
                        );

                    // Using reflection to bypass the private setter on MatchedAILabels
                    typeof(CommunityEntity)
                        .GetProperty(nameof(CommunityEntity.MatchedAILabels))?
                        .SetValue(entity, dto.MatchedAILabels);

                    entities.Add(entity);
                }

                await _context.Set<CommunityEntity>().AddRangeAsync(entities);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Successfully seeded {Count} communities.", entities.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding community data.");
            throw; // Re-throw to prevent silent failures during application startup
        }
    }

    // DTO handles the deserialization, inherently ignoring 'instructorIds' by not mapping it
    private class CommunitySeedDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> MatchedCareers { get; set; } = new();
        public List<int> MatchedAILabels { get; set; } = new();
    }
}