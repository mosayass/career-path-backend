using CareerPath.Community.Core.Entities;

namespace CareerPath.Community.Core.Contracts;

public interface ICommunityMemberRepository
{
    // For CreatePost and AddComment
    Task<bool> IsMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken);

    // For PinPost and EndorseComment
    Task<bool> IsInstructorAsync(Guid communityId, Guid userId, CancellationToken cancellationToken);
    Task AddAsync(CommunityMember member, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<CommunityMember?> GetMembershipAsync(Guid communityId, Guid userId, CancellationToken cancellationToken);
    void Remove(CommunityMember member);
}