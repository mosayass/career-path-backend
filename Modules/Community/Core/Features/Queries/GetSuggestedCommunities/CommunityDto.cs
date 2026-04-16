
namespace CareerPath.Community.Core.Features.Queries.GetSuggestedCommunities
{
    public record CommunityDto(
    Guid Id,
    string Name,
    string Description,
    List<string> MatchedCareers,
    bool IsPrimaryMatch); // Flagged for the UI to highlight
}
