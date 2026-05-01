using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.Entities;
using CareerPath.Community.Core.Enums;

namespace CareerPath.Community.Core.Features.Commands.JoinCommunity;

public class JoinCommunityCommandHandler : IRequestHandler<JoinCommunityCommand, Result<bool>>
{
    private readonly ICommunityRepository _communityRepository;
    private readonly ICommunityMemberRepository _memberRepository;

    public JoinCommunityCommandHandler(
        ICommunityRepository communityRepository,
        ICommunityMemberRepository memberRepository)
    {
        _communityRepository = communityRepository;
        _memberRepository = memberRepository;
    }

    public async Task<Result<bool>> Handle(JoinCommunityCommand request, CancellationToken cancellationToken)
    {
        // 1. Existence Check
        var communityExists = await _communityRepository.ExistsAsync(request.CommunityId, cancellationToken);
        if (!communityExists)
        {
            return Result<bool>.Failure(ErrorType.NotFound, $"Community with ID {request.CommunityId} does not exist.");
        }

        // 2. Idempotency Check
        var isAlreadyMember = await _memberRepository.IsMemberAsync(request.CommunityId, request.UserId, cancellationToken);
        if (isAlreadyMember)
        {
            // Bypassing mutation safely
            return Result<bool>.Success(true);
        }

        // 3. Entity Creation
        var newMember = new CommunityMember(request.UserId, request.CommunityId, CommunityRole.Student);

        // 4. Commit Transaction
        await _memberRepository.AddAsync(newMember, cancellationToken);
        await _memberRepository.SaveChangesAsync(cancellationToken);

        // 5. Return
        return Result<bool>.Success(true);
    }
}