namespace CareerPath.Careers.Core.Entities;

public sealed class Career
{
    public Guid Id { get; private set; }
    public int AiLabelId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string EducationLevel { get; private set; } = string.Empty;

    // Stored as a JSONb column or serialized string in PostgreSQL
    public List<string> CoreSkills { get; private set; } = new();

    // Foreign Key & Navigation
    public int SectorId { get; private set; }
    public CareerSector Sector { get; private set; } = null!;

    // Required by EF Core
    private Career() { }

    public Career(int aiLabelId, string title, string description, string educationLevel, List<string> coreSkills, int sectorId)
    {
        Id = Guid.NewGuid();
        AiLabelId = aiLabelId;
        Title = title;
        Description = description;
        EducationLevel = educationLevel;
        CoreSkills = coreSkills;
        SectorId = sectorId;
    }
}