using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;
using CareerPath.Community.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Community.Infrastructure.Queries;

public class CommunityDiscoveryQueries(CommunityDbContext context) : ICommunityDiscoveryQueries
{
    private readonly CommunityDbContext _context = context;

    public async Task<List<CommunityDto>> GetSuggestedCommunitiesAsync(
        int primaryLabelId,
        List<int> secondaryLabelIds,
        CancellationToken cancellationToken)
    {
        return await _context.Communities
            .AsNoTracking()
            .Where(c => c.MatchedAILabels.Contains(primaryLabelId) ||
                        c.MatchedAILabels.Any(label => secondaryLabelIds.Contains(label)))
            .Select(c => new CommunityDto(
                c.Id,
                c.Name,
                c.Description,
                c.MatchedCareers,
                c.MatchedAILabels.Contains(primaryLabelId), // Dynamically set Primary flag in SQL
                _context.CommunityMembers.Count(m => m.CommunityId == c.Id)
            ))
            .ToListAsync(cancellationToken);
    }
    public async Task<List<CommunityDto>> GetJoinedCommunitiesAsync(Guid userId, CancellationToken cancellationToken)
    {
        // 1. Get the list of CommunityIds this user has joined
        var joinedCommunityIds = _context.CommunityMembers
            .AsNoTracking()
            .Where(cm => cm.UserId == userId && !cm.IsBanned)
            .Select(cm => cm.CommunityId);

        // 2. Fetch the actual Communities using those IDs and project to DTO
        return await _context.Communities
            .AsNoTracking()
            .Where(c => joinedCommunityIds.Contains(c.Id))
            .Select(c => new CommunityDto(
                c.Id,
                c.Name,
                c.Description,
                c.MatchedCareers,
                false, // Explicitly false as this is not an AI match query
                _context.CommunityMembers.Count(m => m.CommunityId == c.Id)
            ))
            .ToListAsync(cancellationToken);
    }
    public async Task<List<CommunityDto>> GetAllCommunitiesAsync(string? searchTerm, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Communities.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var searchPattern = $"%{searchTerm}%";
            query = query.Where(c =>
                EF.Functions.ILike(c.Name, searchPattern) ||
                EF.Functions.ILike(c.Description, searchPattern));
        }

        return await query
            .OrderBy(c => c.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CommunityDto(
                c.Id,
                c.Name,
                c.Description,
                c.MatchedCareers,
                false, // Explicitly false as this is a directory search
                _context.CommunityMembers.Count(m => m.CommunityId == c.Id)
            ))
            .ToListAsync(cancellationToken);
    }
}