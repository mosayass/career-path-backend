using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Repositories;

public class PostRepository(CommunityDbContext context) : IPostRepository
{
    private readonly CommunityDbContext _context = context;

    public async Task AddAsync(Post post, CancellationToken cancellationToken)
    {
        await _context.Posts.AddAsync(post, cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<bool> ExistsAsync(Guid postId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .AnyAsync(p => p.Id == postId, cancellationToken);
    }
    public async Task<bool> BelongsToCommunityAsync(Guid postId, Guid communityId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .AnyAsync(p => p.Id == postId && p.CommunityId == communityId, cancellationToken);
    }
    public async Task<Post?> GetByIdAsync(Guid postId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken);
    }
    public async Task<int> GetPinnedCountByCommunityAsync(Guid communityId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .CountAsync(p => p.CommunityId == communityId && p.IsPinned, cancellationToken);
    }

    public async Task<Post?> GetOldestPinnedPostAsync(Guid communityId, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .Where(p => p.CommunityId == communityId && p.IsPinned)
            .OrderBy(p => p.CreatedAt) // Oldest first
            .FirstOrDefaultAsync(cancellationToken);
    }
}