namespace CareerPath.Community.Core.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid PostId { get; private set; }
    public Guid? ParentCommentId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; set; }
    public string? AuthorAvatarUrl { get; set; }
    public string Body { get; set; }
    public int UpvoteCount { get; set; }
    public int DownvoteCount { get; set; }
    public bool IsInstructorEndorsed { get; set; }
    public DateTime CreatedAt { get; private set; }
    

    // 1. EF Core Constructor
    private Comment() { }

    // 2. Domain Constructor
    public Comment(Guid postId, Guid? parentCommentId, Guid authorId, string authorName, string? authorAvatarUrl, string body)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        ParentCommentId = parentCommentId;
        AuthorId = authorId;
        AuthorName = authorName;
        AuthorAvatarUrl = authorAvatarUrl;
        Body = body;
        UpvoteCount = 0;
        DownvoteCount = 0;
        IsInstructorEndorsed = false;
        CreatedAt = DateTime.UtcNow;
    }
}