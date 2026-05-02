using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;
using CareerPath.Community.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Queries;

public class PostDetailsQueries : IPostDetailsQueries
{
    private readonly CommunityDbContext _context;

    public PostDetailsQueries(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task<PostDto?> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .Where(p => p.Id == postId)
            .Select(p => new PostDto(
                p.Id,
                p.CommunityId,
                p.AuthorId, // Mapped from entity's UserId property
                p.Title,
                p.Body,
                p.MediaUrls,
                p.CareerTag,
                p.AuthorName,
                p.AuthorAvatarUrl,
                p.UpvoteCount,
                p.DownvoteCount,
                _context.Comments.Count(c => c.PostId == p.Id), // Correlated subquery
                p.CreatedAt,
                p.IsPinned
            ))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CommentDto>> GetCommentsByPostIdAsync(Guid postId, CancellationToken cancellationToken)
    {
        return await _context.Comments
            .AsNoTracking()
            .Where(c => c.PostId == postId)
            .Select(c => new CommentDto(
                c.Id,
                c.PostId,
                c.ParentCommentId,
                c.AuthorId, // Passed to AuthorId in the record
                c.Body,
                c.AuthorName,
                c.AuthorAvatarUrl,
                c.UpvoteCount,
                c.DownvoteCount,
                c.IsInstructorEndorsed,
                c.CreatedAt,
                new List<CommentDto>() // Initialize the mutable list for in-memory assembly
            ))
            .ToListAsync(cancellationToken);
    }
}