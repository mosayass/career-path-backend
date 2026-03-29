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
}