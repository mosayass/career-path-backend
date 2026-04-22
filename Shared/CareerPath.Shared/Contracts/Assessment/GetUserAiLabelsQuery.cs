using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Shared.Contracts.Assessment;

public record GetUserAiLabelsQuery(Guid UserId) : IRequest<Result<UserAiLabelsDto>>;