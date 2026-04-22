using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;
using CareerPath.Community.Core.Enums;
using CareerPath.Community.Core.Features.Queries.GetUserPostVotes;
using CareerPath.Community.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Queries;

public class CommunityFeedQueries : ICommunityFeedQueries
{
    private readonly CommunityDbContext _context;

    public CommunityFeedQueries(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task<List<PostDto>> GetPostsAsync(Guid communityId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Posts
            .AsNoTracking()
            .Where(p => p.CommunityId == communityId)
            .OrderByDescending(p => p.IsPinned)
            .ThenByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new PostDto(
                p.Id,
                p.CommunityId,
                p.AuthorId,
                p.Title,
                p.Body,
                p.MediaUrls,
                p.CareerTag,
                p.AuthorName,
                p.AuthorAvatarUrl,
                p.UpvoteCount,
                p.DownvoteCount,
                _context.Comments.Count(c => c.PostId == p.Id), // Correlated subquery using the DbContext
                p.CreatedAt,
                p.IsPinned
            ))
            .ToListAsync(cancellationToken);
    }

    public async Task<List<PostVoteStateDto>> GetUserVotesAsync(Guid userId, List<Guid> postIds, CancellationToken cancellationToken)
    {
        return await _context.Votes
            .AsNoTracking()
            .Where(v => v.UserId == userId
                     && v.TargetType == TargetType.Post
                     && postIds.Contains(v.TargetId))
            .Select(v => new PostVoteStateDto(v.TargetId, v.IsUpvote))
            .ToListAsync(cancellationToken);
    }
}