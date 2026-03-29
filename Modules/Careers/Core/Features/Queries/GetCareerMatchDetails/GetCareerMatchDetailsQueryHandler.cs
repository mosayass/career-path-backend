using CareerPath.Careers.Core.Contracts;
using CareerPath.Shared.Contracts.Careers;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerMatchDetails;

public class GetCareerMatchDetailsQueryHandler : IRequestHandler<GetCareerMatchDetailsQuery, Result<CareerMatchDetailsDto>>
{
    private readonly ICareerRepository _repository;

    public GetCareerMatchDetailsQueryHandler(ICareerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<CareerMatchDetailsDto>> Handle(GetCareerMatchDetailsQuery request, CancellationToken cancellationToken)
    {
        // 1. Fetch primary career
        var primaryCareer = await _repository.GetByAiLabelIdWithSectorAsync(request.AiLabelId, cancellationToken);

        if (primaryCareer == null)
        {
            return Result<CareerMatchDetailsDto>.Failure(
                ErrorType.NotFound,
                $"No career found matching AI Label ID {request.AiLabelId}.");
        }

        // 2. Fetch alternatives
        var alternatives = await _repository.GetAlternativesAsync(primaryCareer.SectorId, primaryCareer.Id, 3, cancellationToken);

        // 3. Map alternatives to DTO
        var alternativeDtos = alternatives.Select(c => new AlternativeCareerDto(
            c.AiLabelId,
            c.Title,
            primaryCareer.Sector.Name // Reusing the sector name since they share it
        )).ToList();

        // 4. Map final response
        var resultDto = new CareerMatchDetailsDto(
            primaryCareer.AiLabelId,
            primaryCareer.Title,
            primaryCareer.Sector.Name,
            primaryCareer.Description,
            primaryCareer.EducationLevel,
            primaryCareer.CoreSkills,
            alternativeDtos
        );

        return Result<CareerMatchDetailsDto>.Success(resultDto);
    }
}