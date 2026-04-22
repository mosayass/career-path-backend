namespace CareerPath.Community.Core.Contracts;

public interface ICommunityMemberRepository
{
    // For CreatePost and AddComment
    Task<bool> IsMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken);

    // For PinPost and EndorseComment
    Task<bool> IsInstructorAsync(Guid communityId, Guid userId, CancellationToken cancellationToken);
}