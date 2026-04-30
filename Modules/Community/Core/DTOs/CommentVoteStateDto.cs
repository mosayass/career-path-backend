namespace CareerPath.Community.Core.DTOs;

public record CommentVoteStateDto(Guid CommentId, bool IsUpvote);