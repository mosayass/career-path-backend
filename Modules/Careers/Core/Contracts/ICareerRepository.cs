using CareerPath.Careers.Core.Entities;

namespace CareerPath.Careers.Core.Contracts;

public interface ICareerRepository
{
    Task<Career?> GetByAiLabelIdWithSectorAsync(
        int aiLabelId,
        CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Career>> GetAlternativesAsync(
        int sectorId,
        Guid excludeCareerId,
        int limit,
        CancellationToken cancellationToken = default);
    Task<(IReadOnlyList<Career> Careers, int TotalCount)> SearchAsync(
        string? searchTerm,
        int? sectorId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
    Task<Career?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

}