using System;
using System.Collections.Generic;
using System.Text;

namespace CareerPath.Community.Core.DTOs
{
    public record PostDto(
    Guid Id,
    Guid CommunityId,
    Guid AuthorId,
    string AuthorName,       
    string? AuthorAvatarUrl,      
    string Title,
    string? Body,
    List<string> MediaUrls,
    string CareerTag,
    int UpvoteCount,
    int DownvoteCount,
    int CommentCount,
    DateTime CreatedAt,
    bool IsPinned);
}
