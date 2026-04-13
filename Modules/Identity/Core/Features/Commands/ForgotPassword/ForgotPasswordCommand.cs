using CareerPath.Shared.Responses;
using MediatR;

namespace CareerPath.Identity.Core.Features.Commands.ForgotPassword;

public record ForgotPasswordCommand(string Email) : IRequest<Result>;