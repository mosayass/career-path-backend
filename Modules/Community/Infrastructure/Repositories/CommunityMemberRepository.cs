using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;
using CareerPath.Community.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Repositories;

public class CommunityMemberRepository : ICommunityMemberRepository
{
    private readonly CommunityDbContext _context;

    public CommunityMemberRepository(CommunityDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsMemberAsync(Guid communityId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.CommunityMembers
            .AnyAsync(cm => cm.CommunityId == communityId
                         && cm.UserId == userId
                         && !cm.IsBanned, cancellationToken);
    }

    public async Task<bool> IsInstructorAsync(Guid communityId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.CommunityMembers
            .AnyAsync(cm => cm.CommunityId == communityId
                         && cm.UserId == userId
                         && cm.Role == CommunityRole.Instructor
                         && !cm.IsBanned, cancellationToken);
    }
    public async Task AddAsync(CommunityMember member, CancellationToken cancellationToken)
    {
        await _context.CommunityMembers.AddAsync(member, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<CommunityMember?> GetMembershipAsync(Guid communityId, Guid userId, CancellationToken cancellationToken)
    {
        return await _context.CommunityMembers
            .FirstOrDefaultAsync(cm => cm.CommunityId == communityId && cm.UserId == userId, cancellationToken);
    }
    public void Remove(CommunityMember member)
    {
        _context.CommunityMembers.Remove(member);
    }
}