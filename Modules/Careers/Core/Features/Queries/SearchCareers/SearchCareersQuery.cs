using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Careers.Core.Features.Queries.SearchCareers;

public record CareerSummaryDto(int AiLabelId, string Title, string Sector, string EducationLevel);

public record PagedCareerResultDto(IReadOnlyList<CareerSummaryDto> Items, int TotalCount, int Page, int PageSize);

public record SearchCareersQuery(string? SearchTerm, int? SectorId, int Page = 1, int PageSize = 20) : IRequest<Result<PagedCareerResultDto>>;