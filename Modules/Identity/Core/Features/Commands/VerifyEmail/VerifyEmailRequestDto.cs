namespace CareerPath.Identity.Core.Features.Commands.VerifyEmail;

public record VerifyEmailRequestDto(string Email, string Code);