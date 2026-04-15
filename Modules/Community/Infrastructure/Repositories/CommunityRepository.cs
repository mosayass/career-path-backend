using Microsoft.EntityFrameworkCore;
using Careerpath.Community.Core.Contracts;
using Careerpath.Community.Infrastructure.Data;

namespace Careerpath.Community.Infrastructure.Repositories;

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