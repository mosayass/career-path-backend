using CareerPath.Identity.Core.DTOs;
using MediatR;

namespace CareerPath.Identity.Core.Features.Commands.Register;

public record RegisterCommand(RegisterRequestDto RequestDto) : IRequest<RegisterResponseDto>;