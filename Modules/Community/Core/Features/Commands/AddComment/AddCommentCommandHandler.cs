using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Shared.Contracts.Profiles;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Community.Core.Features.Commands.AddComment;

public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Result<Guid>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly ISender _sender;

    public AddCommentCommandHandler(
        ICommentRepository commentRepository,
        IPostRepository postRepository,
        ISender sender)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _sender = sender;
    }

    public async Task<Result<Guid>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Target Post
        var postExists = await _postRepository.ExistsAsync(request.PostId, cancellationToken);
        if (!postExists)
        {
            return Result<Guid>.Failure(ErrorType.NotFound, $"Post with ID {request.PostId} does not exist.");
        }

        // 2. Validate Parent Comment (If this is a reply)
        if (request.ParentCommentId.HasValue)
        {
            var parentValid = await _commentRepository.ParentExistsAndBelongsToPostAsync(
                request.ParentCommentId.Value,
                request.PostId,
                cancellationToken);

            if (!parentValid)
            {
                return Result<Guid>.Failure(ErrorType.Validation, "The parent comment either does not exist or belongs to a different post.");
            }
        }

        // 3. Fetch Denormalized Profile Data
        var profileResult = await _sender.Send(new GetProfileDataQuery(request.UserId), cancellationToken);
        if (!profileResult.IsSuccess)
            return Result<Guid>.Failure(profileResult.ErrorType, profileResult.Error);

        var authorName = profileResult.Value.DisplayName;
        var authorAvatarUrl = profileResult.Value.AvatarUrl;

        // 4. Instantiate the Entity
        var comment = new Comment(
            request.PostId,
            request.ParentCommentId,
            request.UserId,
            request.Body,
            authorName,         
            authorAvatarUrl
        );

        // 5. Persist to Database
        await _commentRepository.AddAsync(comment, cancellationToken);
        await _commentRepository.SaveChangesAsync(cancellationToken);

        // 6. Return Result
        return Result<Guid>.Success(comment.Id);
    }
}