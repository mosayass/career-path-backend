using CareerPath.Identity.Core.DTOs;
using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Identity.Core.Features.Queries.Login;

public record LoginQuery(LoginRequestDto RequestDto) : IRequest<Result<LoginResponseDto>>;