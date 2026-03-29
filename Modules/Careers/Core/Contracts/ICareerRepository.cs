using CareerPath.Careers.Core.Entities;

namespace CareerPath.Careers.Core.Contracts;

public interface ICareerRepository
{
    Task<Career?> GetByAiLabelIdWithSectorAsync(int aiLabelId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Career>> GetAlternativesAsync(int sectorId, Guid excludeCareerId, int limit, CancellationToken cancellationToken = default);
}