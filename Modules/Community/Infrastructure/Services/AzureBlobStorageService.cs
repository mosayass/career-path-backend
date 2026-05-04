using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CareerPath.Community.Core.Contracts;
using CareerPath.Community.Core.DTOs;
using Microsoft.Extensions.Configuration;

namespace CareerPath.Community.Infrastructure.Services;

public class AzureBlobStorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private const string ContainerName = "community-posts";
    private readonly IConfiguration _configuration;
    public AzureBlobStorageService(BlobServiceClient blobServiceClient, IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
    }

    public async Task<List<UploadTicketDto>> GeneratePresignedUrlsAsync(List<MediaUploadRequestDto> requests, CancellationToken cancellationToken)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);

        // We only need to ensure the container exists once for the whole batch
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        var tickets = new List<UploadTicketDto>();

        foreach (var request in requests)
        {
            var extension = Path.GetExtension(request.FileName);
            var uniqueBlobName = $"{Guid.NewGuid()}{extension}";
            var blobClient = containerClient.GetBlobClient(uniqueBlobName);

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = ContainerName,
                BlobName = uniqueBlobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15),
                ContentType = request.ContentType
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Write);

            var uploadUrl = blobClient.GenerateSasUri(sasBuilder).ToString();
            var finalUrl = blobClient.Uri.ToString();

            var internalUrl = _configuration["Storage:InternalStorageUrl"];
            var externalUrl = _configuration["Storage:ExternalStorageUrl"];

            // If an external URL is defined and different from the internal one, translate it.
            if (!string.IsNullOrWhiteSpace(internalUrl) && !string.IsNullOrWhiteSpace(externalUrl) && internalUrl != externalUrl)
            {
                uploadUrl = uploadUrl.Replace(internalUrl, externalUrl);
                finalUrl = finalUrl.Replace(internalUrl, externalUrl);
            }

            tickets.Add(new UploadTicketDto(uploadUrl, finalUrl));
        }

        return tickets;
    }

    public async Task InitializeCorsRulesAsync(CancellationToken cancellationToken)
    {
        // This solves the CORS trap for frontend uploads
        var properties = await _blobServiceClient.GetPropertiesAsync(cancellationToken);

        var corsRule = new BlobCorsRule
        {
            MaxAgeInSeconds = 3600,
            AllowedMethods = "PUT, OPTIONS",
            AllowedHeaders = "*",
            ExposedHeaders = "*"
        };

        // In production, restrict this to your frontend URL. Using "*" for local development.
        corsRule.AllowedOrigins = _configuration["Storage:CorsAllowedOrigin"];

        properties.Value.Cors.Clear();
        properties.Value.Cors.Add(corsRule);

        await _blobServiceClient.SetPropertiesAsync(properties.Value, cancellationToken);
    }
}