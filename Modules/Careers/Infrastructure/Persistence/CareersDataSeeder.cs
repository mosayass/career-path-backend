using System.Text.Json;
using CareerPath.Careers.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CareerPath.Careers.Infrastructure.Persistence;

public class CareersDataSeeder
{
    private readonly CareersDbContext _context;
    private readonly ILogger<CareersDataSeeder> _logger;

    public CareersDataSeeder(CareersDbContext context, ILogger<CareersDataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // 1. Idempotency Check & Sector Seeding
            if (!await _context.CareerSectors.AnyAsync())
            {
                _logger.LogInformation("Seeding 19 Career Sectors...");

                var sectors = new List<CareerSector>
                {
                    new(1, "Architecture & Engineering"),
                    new(2, "Arts, Design, Entertainment, Sports, & Media"),
                    new(3, "Business & Financial Operations"),
                    new(4, "Community & Social Service"),
                    new(5, "Computer & Mathematical"),
                    new(6, "Construction & Extraction"),
                    new(7, "Education, Training, & Library"),
                    new(8, "Farming, Fishing, & Forestry"),
                    new(9, "Healthcare Practitioners & Technical"),
                    new(10, "Healthcare Support"),
                    new(11, "Legal"),
                    new(12, "Life, Physical, & Social Science"),
                    new(13, "Management"),
                    new(14, "Office & Administrative Support"),
                    new(15, "Personal Care & Service"),
                    new(16, "Production"),
                    new(17, "Protective Service"),
                    new(18, "Sales & Marketing"),
                    new(19, "Transportation & Material Moving")
                };

                await _context.CareerSectors.AddRangeAsync(sectors);
                await _context.SaveChangesAsync();
            }

            // 2. Idempotency Check & Career JSON Seeding
            if (!await _context.Careers.AnyAsync())
            {
                _logger.LogInformation("Seeding Careers from JSON file...");

                // Construct the path to the copied JSON file
                var filePath = Path.Combine(AppContext.BaseDirectory, "Persistence", "SeedData", "careers.json");

                if (!File.Exists(filePath))
                {
                    _logger.LogWarning("Seed file not found at: {FilePath}", filePath);
                    return;
                }

                var jsonData = await File.ReadAllTextAsync(filePath);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var careersData = JsonSerializer.Deserialize<List<CareerSeedDto>>(jsonData, options);

                if (careersData != null && careersData.Any())
                {
                    // Map the raw JSON DTOs to our rich Domain Entities
                    var careers = careersData.Select(dto => new Career(
                        dto.AiLabelId,
                        dto.Title,
                        dto.Description,
                        dto.EducationLevel,
                        dto.CoreSkills,
                        dto.SectorId
                    )).ToList();

                    await _context.Careers.AddRangeAsync(careers);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Successfully seeded {Count} careers.", careers.Count);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the Careers database.");
            throw;
        }
    }

    // Private DTO strictly used for mapping the JSON file to C# objects
    private class CareerSeedDto
    {
        public int AiLabelId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int SectorId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string EducationLevel { get; set; } = string.Empty;
        public List<string> CoreSkills { get; set; } = new();
    }
}