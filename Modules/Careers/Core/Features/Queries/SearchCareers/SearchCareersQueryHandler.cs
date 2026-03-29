using CareerPath.Careers.Core.Contracts;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Careers.Core.Features.Queries.SearchCareers;

public class SearchCareersQueryHandler : IRequestHandler<SearchCareersQuery, Result<PagedCareerResultDto>>
{
    private readonly ICareerRepository _repository;

    public SearchCareersQueryHandler(ICareerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PagedCareerResultDto>> Handle(SearchCareersQuery request, CancellationToken cancellationToken)
    {
        var (careers, totalCount) = await _repository.SearchAsync(
            request.SearchTerm,
            request.SectorId,
            request.Page,
            request.PageSize,
            cancellationToken);

        // Map domain entities to the lightweight summary DTO
        var dtos = careers.Select(c => new CareerSummaryDto(
            c.AiLabelId,
            c.Title,
            c.Sector.Name,
            c.EducationLevel
        )).ToList();

        var result = new PagedCareerResultDto(dtos, totalCount, request.Page, request.PageSize);

        return Result<PagedCareerResultDto>.Success(result);
    }
}