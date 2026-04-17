using CareerPath.Shared.Responses; 
using MediatR;
using System;

namespace CareerPath.Shared.Contracts.Profiles;

public record GetProfileDataQuery(Guid UserId) : IRequest<Result<ProfileDataResponse>>;