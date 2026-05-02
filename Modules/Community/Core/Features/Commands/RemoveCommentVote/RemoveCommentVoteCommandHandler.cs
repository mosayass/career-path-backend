using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Features.Commands.RemoveCommentVote;

public class RemoveCommentVoteCommandHandler : IRequestHandler<RemoveCommentVoteCommand, Result<bool>>
{
    private readonly IVoteRepository _voteRepository;
    private readonly ICommentRepository _commentRepository;

    public RemoveCommentVoteCommandHandler(
        IVoteRepository voteRepository,
        ICommentRepository commentRepository)
    {
        _voteRepository = voteRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Result<bool>> Handle(RemoveCommentVoteCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the Vote Record
        var vote = await _voteRepository.GetVoteAsync(request.UserId, request.CommentId, TargetType.Comment, cancellationToken);

        // 2. Idempotency Check
        if (vote == null)
        {
            // The user hasn't voted on this comment. Bypass mutation safely.
            return Result<bool>.Success(true);
        }

        // 3. Mutate the Comment Counts
        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment != null)
        {
            if (vote.IsUpvote)
            {
                comment.UpvoteCount--; // Or comment.DecrementUpvote() depending on your Domain Entity encapsulation
            }
            else
            {
                comment.DownvoteCount--; // Or comment.DecrementDownvote()
            }
        }

        // 4. Execute Deletion
        _voteRepository.Delete(vote);

        // 5. Commit Transaction
        await _voteRepository.SaveChangesAsync(cancellationToken);

        // 6. Return
        return Result<bool>.Success(true);
    }
}