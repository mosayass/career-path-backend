namespace CareerPath.Community.Core.Entities;

public class Post
{
    public Guid Id { get; private set; }
    public Guid CommunityId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; set; }
    public string? AuthorAvatarUrl { get; set; }
    public string Title { get; private set; }
    public string? Body { get; private set; }
    public string CareerTag { get;  set; }
    public int UpvoteCount { get;  set; }
    public int DownvoteCount { get;  set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsPinned { get; set; }
    public List<string> MediaUrls { get; set; }

    // 1. EF Core Constructor
    private Post() { }

    // 2. Domain Constructor
    public Post(Guid communityId, Guid authorId, string authorName, string? authorAvatarUrl, string title, string? body, string careerTag, List<string> mediaUrls)
    {
        Id = Guid.NewGuid();
        CommunityId = communityId;
        AuthorId = authorId;
        AuthorName = authorName;
        AuthorAvatarUrl = authorAvatarUrl;
        Title = title;
        Body = body;
        CareerTag = careerTag;
        UpvoteCount = 0;
        DownvoteCount = 0;
        CreatedAt = DateTime.UtcNow;
        IsPinned = false;
        MediaUrls = mediaUrls; 
    }
}