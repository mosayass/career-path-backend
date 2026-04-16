using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Features.Commands.CastPostVote;

/* * FOR THE COMMENT VERSION (CastCommentVoteCommandHandler):
 * 1. Change IPostRepository to ICommentRepository.
 * 2. Fetch the comment using _commentRepository.GetByIdAsync().
 * 3. Change TargetType.Post to TargetType.Comment when instantiating or querying Votes.
 * 4. Update the variables and error messages from "post" to "comment".
 */

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
        // 1. Fetch the Target Post
        var post = await _postRepository.GetByIdAsync(request.TargetId, cancellationToken);
        if (post == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Post with ID {request.TargetId} does not exist.");
        }

        // 2. Check for an Existing Vote
        var existingVote = await _voteRepository.GetVoteAsync(request.UserId, request.TargetId, TargetType.Post, cancellationToken);

        // 3. Scenario A: User has never voted on this post
        if (existingVote == null)
        {
            var newVote = new Vote(request.UserId, request.TargetId, TargetType.Post, request.IsUpvote);
            await _voteRepository.AddAsync(newVote, cancellationToken);

            if (request.IsUpvote) post.UpvoteCount++;
            else post.DownvoteCount++;
        }
        // 4. Scenario B: User is toggling their existing vote off
        else if (existingVote.IsUpvote == request.IsUpvote)
        {
            _voteRepository.Delete(existingVote);

            if (request.IsUpvote) post.UpvoteCount--;
            else post.DownvoteCount--;
        }
        // 5. Scenario C: User is switching their vote (Up to Down, or Down to Up)
        else
        {
            existingVote.IsUpvote = request.IsUpvote; // Mutate the vote state

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

        // 6. Commit Transaction (Updates both Post and Vote tables atomically)
        await _voteRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}