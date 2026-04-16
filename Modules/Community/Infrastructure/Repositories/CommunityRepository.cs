using Microsoft.EntityFrameworkCore;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Infrastructure.Persistence;

namespace CareerPath.Community.Infrastructure.Repositories;

public class CommunityRepository(CommunityDbContext context) : ICommunityRepository
{
    private readonly CommunityDbContext _context = context;

    public async Task<bool> ExistsAsync(Guid communityId, CancellationToken cancellationToken)
    {
        return await _context.Communities
            .AsNoTracking()
            .AnyAsync(c => c.Id == communityId, cancellationToken);
    }
    public async Task<bool> HasInstructorAsync(Guid communityId, Guid instructorId, CancellationToken cancellationToken)
    {
        return await _context.Communities
            .AsNoTracking()
            .AnyAsync(c => c.Id == communityId && c.InstructorIds.Contains(instructorId), cancellationToken);
    }
}