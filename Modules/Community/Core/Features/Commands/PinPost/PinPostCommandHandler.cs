using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;

namespace CareerPath.Community.Core.Features.Commands.PinPost;

public class PinPostCommandHandler : IRequestHandler<PinPostCommand, Result<bool>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICommunityMemberRepository _memberRepository;

    public PinPostCommandHandler(
        IPostRepository postRepository,
        ICommunityMemberRepository memberRepository)
    {
        _postRepository = postRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<bool>> Handle(PinPostCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Moderation Rights (using UserId)
        var isAuthorized = await _memberRepository.IsInstructorAsync(request.CommunityId, request.UserId, cancellationToken);
        if (!isAuthorized)
        {
            return Result<bool>.Failure(ErrorType.Forbidden, "You are not an assigned instructor for this community.");
        }

        // 2. Fetch Target
        var targetPost = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (targetPost == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Post with ID {request.PostId} does not exist.");
        }

        // 3. Boundary Check
        if (targetPost.CommunityId != request.CommunityId)
        {
            return Result<bool>.Failure(ErrorType.Validation, "This post does not belong to the specified community.");
        }

        // Idempotency check: If it's already pinned, just return success
        if (targetPost.IsPinned) return Result<bool>.Success(true);

        // 4. Count Check
        var pinnedCount = await _postRepository.GetPinnedCountByCommunityAsync(request.CommunityId, cancellationToken);

        // 5. The FIFO Swap
        if (pinnedCount >= 3)
        {
            var oldestPost = await _postRepository.GetOldestPinnedPostAsync(request.CommunityId, cancellationToken);
            oldestPost?.IsPinned = false;
        }

        // 6. Mutate Target
        targetPost.IsPinned = true;

        // 7. Commit Transaction
        await _postRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}