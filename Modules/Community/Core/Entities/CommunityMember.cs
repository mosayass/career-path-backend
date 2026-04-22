using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Entities;

public class CommunityMember
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CommunityId { get; private set; }
    public CommunityRole Role { get; set; }
    public bool IsBanned { get; set; }
    public DateTime JoinedAt { get; private set; }

    private CommunityMember() { }

    public CommunityMember(Guid userId, Guid communityId, CommunityRole role)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CommunityId = communityId;
        Role = role;
        IsBanned = false; // For future implementation
        JoinedAt = DateTime.UtcNow;
    }
}