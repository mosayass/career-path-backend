using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Entities;

public class Vote
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid TargetId { get; private set; }
    public TargetType TargetType { get; private set; }
    public bool IsUpvote { get; set; }

    // 1. EF Core Constructor
    private Vote() { }

    // 2. Domain Constructor
    public Vote(Guid userId, Guid targetId, TargetType targetType, bool isUpvote)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TargetId = targetId;
        TargetType = targetType;
        IsUpvote = isUpvote;
    }
}