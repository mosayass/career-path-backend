namespace CareerPath.Community.Core.DTOs;

public record PostDto(
    Guid Id,
    Guid CommunityId,
    Guid AuthorId,
    string Title,
    string? Body,
    List<string> MediaUrls,
    string CareerTag,
    string AuthorName,
    string? AuthorAvatarUrl,
    int UpvoteCount,
    int DownvoteCount,
    int CommentCount,
    DateTime CreatedAt,
    bool IsPinned);