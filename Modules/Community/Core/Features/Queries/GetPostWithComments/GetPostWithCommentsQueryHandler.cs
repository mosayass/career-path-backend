using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetPostWithComments;

public class GetPostWithCommentsQueryHandler : IRequestHandler<GetPostWithCommentsQuery, Result<PostDetailsDto>>
{
    private readonly IPostDetailsQueries _queries;

    public GetPostWithCommentsQueryHandler(IPostDetailsQueries queries)
    {
        _queries = queries;
    }

    public async Task<Result<PostDetailsDto>> Handle(GetPostWithCommentsQuery request, CancellationToken cancellationToken)
    {
        // 1. Flat Fetch: Post
        var post = await _queries.GetPostByIdAsync(request.PostId, cancellationToken);
        if (post == null)
        {
            return Result<PostDetailsDto>.Failure(ErrorType.NotFound, $"Post with ID {request.PostId} does not exist.");
        }

        // 2. Flat Fetch: All Comments
        var flatComments = await _queries.GetCommentsByPostIdAsync(request.PostId, cancellationToken);

        // 3. Assemble Tree
        var commentTree = BuildCommentTree(flatComments);

        // 4. Return Result
        var postDetails = new PostDetailsDto(post, commentTree);
        return Result<PostDetailsDto>.Success(postDetails);
    }

    private List<CommentDto> BuildCommentTree(List<CommentDto> flatComments)
    {
        // Use a dictionary for O(1) lookups
        var commentDictionary = flatComments.ToDictionary(c => c.Id);
        var rootComments = new List<CommentDto>();

        // Build the relationships
        foreach (var comment in flatComments)
        {
            if (comment.ParentCommentId.HasValue)
            {
                if (commentDictionary.TryGetValue(comment.ParentCommentId.Value, out var parent))
                {
                    parent.Replies.Add(comment);
                }
            }
            else
            {
                // Level 1 Comment
                rootComments.Add(comment);
            }
        }

        // Sort the nested replies recursively (Chronological)
        SortReplies(rootComments);

        // Sort Level 1 comments (Popularity: Upvotes - Downvotes)
        return rootComments
            .OrderByDescending(c => c.IsInstructorEndorsed)
            .ThenByDescending(c => c.UpvoteCount - c.DownvoteCount)
            .ThenByDescending(c => c.CreatedAt) // Fallback to newest if votes are tied
            .ToList();
    }

    private void SortReplies(List<CommentDto> comments)
    {
        foreach (var comment in comments)
        {
            if (comment.Replies.Any())
            {
                // Sort replies chronologically
                comment.Replies.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                // Recurse for Level 3+
                SortReplies(comment.Replies);
            }
        }
    }
}