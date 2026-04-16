namespace CareerPath.Community.Core.Contracts;

public interface ICommunityRepository
{
    Task<bool> ExistsAsync(Guid communityId, CancellationToken cancellationToken);
    Task<bool> HasInstructorAsync(Guid communityId, Guid instructorId, CancellationToken cancellationToken);
}