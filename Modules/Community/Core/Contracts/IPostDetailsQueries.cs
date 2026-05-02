using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Contracts;

public interface IPostDetailsQueries
{
    Task<PostDto?> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken);
    Task<List<CommentDto>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken);
}