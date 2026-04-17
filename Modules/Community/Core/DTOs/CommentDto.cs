using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.DTOs
{
    public record CommentDto(
    Guid Id,
    Guid PostId,
    Guid? ParentCommentId,
    Guid AuthorId,
    string Body,
    int UpvoteCount,
    int DownvoteCount,
    bool IsInstructorEndorsed,
    DateTime CreatedAt,
    List<CommentDto> Replies); // Recursive for Level 2+ threads
}
