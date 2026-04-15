using Careerpath.Community.Core.Entities;

namespace Careerpath.Community.Core.Contracts;

public interface IPostRepository
{
    Task AddAsync(Post post, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid postId, CancellationToken cancellationToken);
    Task<bool> BelongsToCommunityAsync(Guid postId, Guid communityId, CancellationToken cancellationToken);
}