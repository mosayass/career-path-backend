using Careerpath.Community.Core.Contracts;
using CareerPath.Community.Core.Features.Commands.EndorseComment;
using CareerPath.Shared.Responses;
using MediatR;

namespace Careerpath.Community.Core.Features.Commands.EndorseComment;

public class EndorseCommentCommandHandler : IRequestHandler<EndorseCommentCommand, Result<bool>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly IPostRepository _postRepository;

    public EndorseCommentCommandHandler(
        ICommentRepository commentRepository,
        ICommunityRepository communityRepository,
        IPostRepository postRepository)
    {
        _commentRepository = commentRepository;
        _communityRepository = communityRepository;
        _postRepository = postRepository;
    }

    public async Task<Result<bool>> Handle(EndorseCommentCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate the Instructor belongs to the Community
        var isAuthorizedInstructor = await _communityRepository.HasInstructorAsync(request.CommunityId, request.InstructorId, cancellationToken);
        if (!isAuthorizedInstructor)
        {
            return Result<bool>.Failure(ErrorType.Forbidden, "You are not an assigned instructor for this community.");
        }

        // 2. Fetch the Comment
        var comment = await _commentRepository.GetByIdAsync(request.CommentId, cancellationToken);
        if (comment == null)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Comment with ID {request.CommentId} does not exist.");
        }

        // 3. Prevent Cross-Boundary Endorsements
        var belongsToCommunity = await _postRepository.BelongsToCommunityAsync(comment.PostId, request.CommunityId, cancellationToken);
        if (!belongsToCommunity)
        {
            return Result<bool>.Failure(ErrorType.Validation, "This comment does not belong to a post within your assigned community.");
        }

        // 4. Mutate State
        comment.IsInstructorEndorsed = true;

        // 5. Commit to Database
        await _commentRepository.SaveChangesAsync(cancellationToken);

        // 6. Return Result
        return Result<bool>.Success(true);
    }
}