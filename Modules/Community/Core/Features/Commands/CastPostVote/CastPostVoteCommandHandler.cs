using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Features.Commands.CastPostVote;

public class CastPostVoteCommandHandler : IRequestHandler<CastPostVoteCommand, Result<bool>>
{
    private readonly IVoteRepository _voteRepository;
    private readonly IPostRepository _postRepository;

    public CastPostVoteCommandHandler(
        IVoteRepository voteRepository,
        IPostRepository postRepository)
    {
        _voteRepository = voteRepository;
        _postRepository = postRepository;
    }

    public async Task<Result<bool>> Handle(CastPostVoteCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch Target Post
        var post = await _postRepository.GetByIdAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Post with ID {request.PostId} does not exist.");
        }

        // 2. Fetch Existing Vote
        var existingVote = await _voteRepository.GetVoteAsync(request.UserId, request.PostId, TargetType.Post, cancellationToken);

        // 3. Idempotent Check: If vote exists and matches request, do nothing and return Success
        if (existingVote != null && existingVote.IsUpvote == request.IsUpvote)
        {
            return Result<bool>.Success(true);
        }

        // 4. Update Logic (Changing mind)
        if (existingVote != null && existingVote.IsUpvote != request.IsUpvote)
        {
            existingVote.IsUpvote = request.IsUpvote;

            if (request.IsUpvote)
            {
                post.DownvoteCount--;
                post.UpvoteCount++;
            }
            else
            {
                post.UpvoteCount--;
                post.DownvoteCount++;
            }
        }
        // 5. Add Logic (New vote)
        else if (existingVote == null)
        {
            var newVote = new Vote(request.UserId, request.PostId, TargetType.Post, request.IsUpvote);
            await _voteRepository.AddAsync(newVote, cancellationToken);

            if (request.IsUpvote)
            {
                post.UpvoteCount++;
            }
            else
            {
                post.DownvoteCount++;
            }
        }

        // 6. Commit Transaction
        await _voteRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}