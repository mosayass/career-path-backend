using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Contracts;

public interface IVoteRepository
{
    Task<Vote?> GetVoteAsync(Guid userId, Guid targetId, TargetType targetType, CancellationToken cancellationToken);
    Task AddAsync(Vote vote, CancellationToken cancellationToken);
    void Delete(Vote vote);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}