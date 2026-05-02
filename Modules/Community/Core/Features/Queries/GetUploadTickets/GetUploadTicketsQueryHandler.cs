using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetUploadTickets;

public class GetUploadTicketsQueryHandler : IRequestHandler<GetUploadTicketsQuery, Result<List<UploadTicketDto>>>
{
    private readonly IStorageService _storageService;

    public GetUploadTicketsQueryHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<Result<List<UploadTicketDto>>> Handle(GetUploadTicketsQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _storageService.GeneratePresignedUrlsAsync(request.Files, cancellationToken);
        return Result<List<UploadTicketDto>>.Success(tickets);
    }
}