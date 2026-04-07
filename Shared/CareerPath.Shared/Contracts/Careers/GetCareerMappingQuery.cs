using MediatR;

namespace CareerPath.Shared.Contracts.Careers
{
    // Takes the int from the AI and asks for the DTO back
    public record GetCareerMappingQuery(int AiLabelId) : IRequest<CareerMappingDto?>;
}