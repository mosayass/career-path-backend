using CareerPath.Careers.Core.Contracts;
using CareerPath.Shared.Contracts.Careers;
using CareerPath.Shared.Responses;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerById;

public class GetCareerSummaryByIdQueryHandler(ICareerRepository careerRepository)
    : IRequestHandler<GetCareerSummaryByIdQuery, Result<CareerSummaryDto>>
{
    public async Task<Result<CareerSummaryDto>> Handle(GetCareerSummaryByIdQuery request, CancellationToken cancellationToken)
    {
        // 1. Repository remains pure: fetches the Domain Entity
        var career = await careerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (career == null)
        {
            return Result<CareerSummaryDto>.Failure(
                ErrorType.NotFound,
                $"Career with ID {request.Id} was not found.");
        }

        // 2. Application Layer handles the mapping to the DTO
        var dto = new CareerSummaryDto(
            CareerId: career.Id,
            Title: career.Title,
            Sector: career.Sector?.Name ?? "Unknown Sector"
        );

        return Result<CareerSummaryDto>.Success(dto);
    }
}