using Microsoft.EntityFrameworkCore;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;
using CareerPath.Community.Infrastructure.Persistence;

namespace CareerPath.Community.Infrastructure.Repositories;

public class VoteRepository(CommunityDbContext context) : IVoteRepository
{
    private readonly CommunityDbContext _context = context;

    public async Task<Vote?> GetVoteAsync(Guid userId, Guid targetId, TargetType targetType, CancellationToken cancellationToken)
    {
        return await _context.Votes
            .FirstOrDefaultAsync(v => v.UserId == userId && v.TargetId == targetId && v.TargetType == targetType, cancellationToken);
    }

    public async Task AddAsync(Vote vote, CancellationToken cancellationToken)
    {
        await _context.Votes.AddAsync(vote, cancellationToken);
    }

    public void Delete(Vote vote)
    {
        _context.Votes.Remove(vote);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}