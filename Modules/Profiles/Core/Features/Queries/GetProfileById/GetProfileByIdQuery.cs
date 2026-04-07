using System;
using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Profiles.Core.Features.Queries.GetProfileById
{
    public record GetProfileByIdQuery(Guid UserId) : IRequest<Result<UserProfileResponse>>;
}