namespace CareerPath.Community.Core.DTOs
{
    public record CommunityDto(
    Guid Id,
    string Name,
    string Description,
    List<string> MatchedCareers,
    bool IsPrimaryMatch, // Flagged for the UI to highlight
    int MemberCount);// new FOR Fronted
}
