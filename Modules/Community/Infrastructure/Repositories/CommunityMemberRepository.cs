using CareerPath.Community.Core.Contracts;
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
}