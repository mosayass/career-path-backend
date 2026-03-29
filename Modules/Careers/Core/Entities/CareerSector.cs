namespace CareerPath.Careers.Core.Entities;

public sealed class CareerSector
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    // Navigation property mapping
    private readonly List<Career> _careers = new();
    public IReadOnlyCollection<Career> Careers => _careers.AsReadOnly();

    // Required by EF Core
    private CareerSector() { }

    public CareerSector(int id, string name)
    {
        Id = id;
        Name = name;
    }
}