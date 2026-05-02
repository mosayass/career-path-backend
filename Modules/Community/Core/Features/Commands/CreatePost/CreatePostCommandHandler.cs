using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Shared.Responses;
using MediatR;
 using CareerPath.Shared.Contracts.Profiles;

namespace CareerPath.Community.Core.Features.Commands.CreatePost;

public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result<Guid>>
{
    private readonly IPostRepository _postRepository;
    private readonly ICommunityRepository _communityRepository;
    private readonly ISender _sender;

    public CreatePostCommandHandler(
        IPostRepository postRepository,
        ICommunityRepository communityRepository,
        ISender sender)
    {
        _postRepository = postRepository;
        _communityRepository = communityRepository;
        _sender = sender;   
    }

    public async Task<Result<Guid>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate Community exists (Lightweight Read)
        var communityExists = await _communityRepository.ExistsAsync(request.CommunityId, cancellationToken);
        if (!communityExists)
        {
            return Result<Guid>.Failure(ErrorType.NotFound, $"Community with ID {request.CommunityId} does not exist.");
        }

        // 2. Fetch Denormalized Data from Profiles Module (Synchronous Cross-Module Call)
        
        var profileResult = await _sender.Send(new GetProfileDataQuery(request.UserId), cancellationToken);
        if (!profileResult.IsSuccess)
        {
            return Result<Guid>.Failure(profileResult.ErrorType, profileResult.Error);
        }
        
        var authorName = profileResult.Value.DisplayName;
        var authorAvatarUrl = profileResult.Value.AvatarUrl;
        

        // 3. Instantiate the Entity safely via the parameterized constructor
        var post = new Post(
            request.CommunityId,
            request.UserId,
            authorName,
            authorAvatarUrl,
            request.Title,
            request.Body,
            request.CareerTag,
            request.MediaUrls
        // Note: Make sure your Post constructor is updated to accept the denormalized data and MediaUrls as agreed.
        );

        // 4. Persist to Database
        await _postRepository.AddAsync(post, cancellationToken);
        await _postRepository.SaveChangesAsync(cancellationToken);

        // 5. Return the Result
        return Result<Guid>.Success(post.Id);
    }
}