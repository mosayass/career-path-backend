using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Shared.Contracts.Careers;

public record GetCareerMatchDetailsQuery(int AiLabelId) : IRequest<Result<CareerMatchDetailsDto>>;