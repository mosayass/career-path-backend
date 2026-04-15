using Microsoft.EntityFrameworkCore;
using Careerpath.Community.Core.Contracts;
using Careerpath.Community.Core.Entities;
using Careerpath.Community.Infrastructure.Data;

namespace Careerpath.Community.Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly CommunityDbContext _context;

    public CommentRepository(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Comment comment, CancellationToken cancellationToken)
    {
        await _context.Comments.AddAsync(comment, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ParentExistsAndBelongsToPostAsync(Guid parentCommentId, Guid postId, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .AsNoTracking()
            .AnyAsync(c => c.Id == parentCommentId && c.PostId == postId, cancellationToken);
    }
    public async Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId, cancellationToken);
    }
}