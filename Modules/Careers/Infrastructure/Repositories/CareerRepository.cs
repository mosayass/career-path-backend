using CareerPath.Careers.Core.Contracts;
using CareerPath.Careers.Core.Entities;
using CareerPath.Careers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CareerPath.Careers.Infrastructure.Repositories;

public class CareerRepository(CareersDbContext context) : ICareerRepository
{
    private readonly CareersDbContext _context = context;

    public async Task<Career?> GetByAiLabelIdWithSectorAsync(int aiLabelId, CancellationToken cancellationToken = default)
    {
        return await _context.Careers
            .Include(c => c.Sector)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.AiLabelId == aiLabelId, cancellationToken);
    }

    public async Task<IReadOnlyList<Career>> GetAlternativesAsync(int sectorId, Guid excludeCareerId, int limit, CancellationToken cancellationToken = default)
    {
        return await _context.Careers
            .Where(c => c.SectorId == sectorId && c.Id != excludeCareerId)
            .AsNoTracking()
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
    public async Task<(IReadOnlyList<Career> Careers, int TotalCount)> SearchAsync(
        string? searchTerm, int? sectorId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _context.Careers
            .Include(c => c.Sector)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            // Case-insensitive search on the job title
            var lowerSearchTerm = searchTerm.ToLower();
            query = query.Where(c => c.Title.ToLower().Contains(lowerSearchTerm));
        }

        if (sectorId.HasValue)
        {
            query = query.Where(c => c.SectorId == sectorId.Value);
        }

        // Get the total count before applying pagination (needed for frontend UI)
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var careers = await query
            .OrderBy(c => c.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (careers, totalCount);
    }
}