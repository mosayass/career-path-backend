using CareerPath.Shared.Contracts.Careers;
using CareerPath.Shared.Responses; 
using MediatR;

namespace CareerPath.Careers.Core.Features.Queries.GetCareerById;

public record GetCareerSummaryByIdQuery(Guid Id) : IRequest<Result<CareerSummaryDto>>;