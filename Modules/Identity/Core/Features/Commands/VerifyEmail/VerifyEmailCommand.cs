using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Identity.Core.Features.Commands.VerifyEmail;

public record VerifyEmailCommand(VerifyEmailRequestDto RequestDto) : IRequest<Result>;