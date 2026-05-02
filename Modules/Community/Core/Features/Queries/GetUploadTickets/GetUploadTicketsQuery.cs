using MediatR;
using CareerPath.Shared.Responses;
using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Features.Queries.GetUploadTickets;

public record GetUploadTicketsQuery(
    List<MediaUploadRequestDto> Files) : IRequest<Result<List<UploadTicketDto>>>;