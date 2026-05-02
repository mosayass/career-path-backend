using MediatR;
using CareerPath.Shared.Responses;

namespace CareerPath.Community.Core.Features.Queries.GetUserPostVotes;

public record GetUserPostVotesQuery(
    Guid UserId,
    List<Guid> PostIds) : IRequest<Result<List<PostVoteStateDto>>>;