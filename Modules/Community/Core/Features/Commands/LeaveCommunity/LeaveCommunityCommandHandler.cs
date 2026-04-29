using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;

namespace CareerPath.Community.Core.Features.Commands.LeaveCommunity;

public class LeaveCommunityCommandHandler : IRequestHandler<LeaveCommunityCommand, Result<bool>>
{
    private readonly ICommunityMemberRepository _memberRepository;

    public LeaveCommunityCommandHandler(ICommunityMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Result<bool>> Handle(LeaveCommunityCommand request, CancellationToken cancellationToken)
    {
        // 1. Fetch the Membership Record
        var membership = await _memberRepository.GetMembershipAsync(request.CommunityId, request.UserId, cancellationToken);

        // 2. Idempotency Check
        if (membership == null)
        {
            // The user is already not a member. Bypass mutation safely.
            return Result<bool>.Success(true);
        }

        // 3. Execute Deletion
        _memberRepository.Remove(membership);

        // 4. Commit Transaction
        await _memberRepository.SaveChangesAsync(cancellationToken);

        // 5. Return
        return Result<bool>.Success(true);
    }
}