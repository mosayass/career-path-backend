using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Contracts;

public interface ICommunityDiscoveryQueries
{
    Task<List<CommunityDto>> GetSuggestedCommunitiesAsync(
        int primaryLabelId,
        List<int> secondaryLabelIds,
        CancellationToken cancellationToken);
    Task<List<CommunityDto>> GetJoinedCommunitiesAsync(
        Guid userId,
        CancellationToken cancellationToken);
    Task<List<CommunityDto>> GetAllCommunitiesAsync(
        string? searchTerm,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken);
}