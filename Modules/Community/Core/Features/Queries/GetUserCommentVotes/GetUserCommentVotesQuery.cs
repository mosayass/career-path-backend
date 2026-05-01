using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetUserCommentVotes;

public record GetUserCommentVotesQuery(
    Guid UserId,
    List<Guid> CommentIds) : IRequest<Result<List<CommentVoteStateDto>>>;