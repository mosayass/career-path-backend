using CareerPath.Identity.Core.DTOs;
using MediatR;

namespace CareerPath.Identity.Core.Features.Queries.Login;

public record LoginQuery(LoginRequestDto RequestDto) : IRequest<LoginResponseDto>;