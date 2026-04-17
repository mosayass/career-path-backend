using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;
using CareerPath.Community.Core.Features.Commands.CastCommentVote;
using CareerPath.Shared.Responses;
using MediatR;

namespace Careerpath.Community.Core.Features.Commands.CastCommentVote;

public class CastCommentVoteCommandHandler : IRequestHandler<CastCommentVoteCommand, Result<bool>>
{
    private readonly IVoteRepository _voteRepository;
    private readonly ICommentRepository _commentRepository;

    public CastCommentVoteCommandHandler(
        IVoteRepository voteRepository,
        ICommentRepository commentRepository)
    {
        _voteRepository = voteRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Result<bool>> Handle(CastCommentVoteCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the Target Comment
        var comment = await _commentRepository.GetByIdAsync(request.TargetId, cancellationToken);
        if (comment == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Comment with ID {request.TargetId} does not exist.");
        }

        // 2. Check for an Existing Vote
        var existingVote = await _voteRepository.GetVoteAsync(request.UserId, request.TargetId, TargetType.Comment, cancellationToken);

        // 3. Scenario A: User has never voted on this comment
        if (existingVote == null)
        {
            var newVote = new Vote(request.UserId, request.TargetId, TargetType.Comment, request.IsUpvote);
            await _voteRepository.AddAsync(newVote, cancellationToken);

            if (request.IsUpvote) comment.UpvoteCount++;
            else comment.DownvoteCount++;
        }
        // 4. Scenario B: User is toggling their existing vote off
        else if (existingVote.IsUpvote == request.IsUpvote)
        {
            _voteRepository.Delete(existingVote);

            if (request.IsUpvote) comment.UpvoteCount--;
            else comment.DownvoteCount--;
        }
        // 5. Scenario C: User is switching their vote (Up to Down, or Down to Up)
        else
        {
            existingVote.IsUpvote = request.IsUpvote; // Mutate the vote state

            if (request.IsUpvote)
            {
                comment.DownvoteCount--;
                comment.UpvoteCount++;
            }
            else
            {
                comment.UpvoteCount--;
                comment.DownvoteCount++;
            }
        }

        // 6. Commit Transaction (Updates both Comment and Vote tables atomically)
        await _voteRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}