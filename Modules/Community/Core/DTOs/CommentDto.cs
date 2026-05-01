namespace CareerPath.Community.Core.DTOs;

public record CommentDto(
    Guid Id,
    Guid PostId,
    Guid? ParentCommentId,
    Guid UserId,
    string Body,
    string AuthorName,
    string? AuthorAvatarUrl,
    int UpvoteCount,
    int DownvoteCount,
    bool IsInstructorEndorsed,
    DateTime CreatedAt,
    List<CommentDto> Replies);