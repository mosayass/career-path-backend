namespace CareerPath.Community.Core.Entities;

public class CommunityEntity
{
    public Guid Id { get; private set; }
    public string Name { get; set; }
    public string Description { get;  set; }
    public List<string> MatchedCareers { get; set; }
    public List<int> MatchedAILabels { get; private set; }
    public List<Guid> InstructorIds { get; set; }

    // 1. EF Core Constructor
    private CommunityEntity() { }

    // 2. Domain Constructor
    public CommunityEntity(string name, string description, List<string> matchedCareers, List<int> matchedAILabels, List<Guid> instructorIds)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        MatchedCareers = matchedCareers;
        MatchedAILabels = matchedAILabels ?? new List<int>();
        InstructorIds = instructorIds ?? new List<Guid>();
    }
}