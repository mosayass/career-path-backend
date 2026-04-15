using Careerpath.Community.Core.Contracts;
using Careerpath.Community.Core.Entities;
using Careerpath.Community.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Careerpath.Community.Infrastructure.Repositories;

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
}