using System.Threading;
using System.Threading.Tasks;
using MediatR;
using CareerPath.Shared.Contracts.Careers;
using CareerPath.Careers.Core.Contracts;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerMapping
{
    public class GetCareerMappingQueryHandler : IRequestHandler<GetCareerMappingQuery, CareerMappingDto?>
    {
        private readonly ICareerRepository _careerRepository;

        public GetCareerMappingQueryHandler(ICareerRepository careerRepository)
        {
            _careerRepository = careerRepository;
        }

        public async Task<CareerMappingDto?> Handle(GetCareerMappingQuery request, CancellationToken cancellationToken)
        {
            // request.AiLabelId is the integer returned from the AI model
            var career = await _careerRepository.GetByAiLabelIdWithSectorAsync(request.AiLabelId, cancellationToken);

            if (career == null)
            {
                return null;
            }

            return new CareerMappingDto(career.SectorId, career.Id);
        }
    }
}