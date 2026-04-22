namespace CareerPath.Community.Core.Contracts;

public interface ICommunityRepository
{
    Task<bool> ExistsAsync(Guid communityId, CancellationToken cancellationToken);
}