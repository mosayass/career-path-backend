namespace CareerPath.Identity.Core.Features.Commands.ChangeRoleToMentor;

using CareerPath.Shared.Responses;
using MediatR;

public record ChangeRoleToMentorCommand(string Email) : IRequest<Result>;