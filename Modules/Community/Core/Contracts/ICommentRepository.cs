using Careerpath.Community.Core.Entities;

namespace Careerpath.Community.Core.Contracts;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    // Ensures the parent comment exists AND belongs to the exact same post
    Task<bool> ParentExistsAndBelongsToPostAsync(Guid parentCommentId, Guid postId, CancellationToken cancellationToken);
    Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken);
}