using CareerPath.Community.Core.DTOs;

namespace CareerPath.Community.Core.Contracts;

public interface IStorageService
{
    Task<List<UploadTicketDto>> GeneratePresignedUrlsAsync(
        List<MediaUploadRequestDto> requests,
        CancellationToken cancellationToken);
    Task InitializeCorsRulesAsync(
        CancellationToken cancellationToken); // The CORS fix
}