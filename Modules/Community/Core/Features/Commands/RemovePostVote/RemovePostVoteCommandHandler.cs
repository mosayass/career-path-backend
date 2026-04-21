using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Features.Commands.RemovePostVote;

public class RemovePostVoteCommandHandler : IRequestHandler<RemovePostVoteCommand, Result<bool>>
{
    private readonly IVoteRepository _voteRepository;
    private readonly IPostRepository _postRepository;

    public RemovePostVoteCommandHandler(
        IVoteRepository voteRepository,
        IPostRepository postRepository)
    {
        _voteRepository = voteRepository;
        _postRepository = postRepository;
    }

    public async Task<Result<bool>> Handle(RemovePostVoteCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch Target Post
        var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Post with ID {request.PostId} does not exist.");
        }

        // 2. Fetch Existing Vote
        var existingVote = await _voteRepository.GetVoteAsync(request.UserId, request.PostId, TargetType.Post, cancellationToken);

        // 3. Idempotent Check: If the vote does not exist, there is nothing to remove, return Success
        if (existingVote == null)
        {
            return Result<bool>.Success(true);
        }

        // 4. Remove Logic
        _voteRepository.Delete(existingVote);

        if (existingVote.IsUpvote)
        {
            post.UpvoteCount--;
        }
        else
        {
            post.DownvoteCount--;
        }

        // 5. Commit Transaction
        await _voteRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}