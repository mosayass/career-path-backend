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
            sasBuilder.SetPermissions(BlobSasPermissions.Add | BlobSasPermissions.Write);

            var uploadUrl = blobClient.GenerateSasUri(sasBuilder).ToString();
            var finalUrl = blobClient.Uri.ToString();

            // Safely read the boolean flag
            var isLocal = _configuration.GetValue<bool>("Storage:IsLocalContainer", false);

            // ONLY perform the translation hack if we are running the local Azurite container
            if (isLocal)
            {
                var internalUrl = _configuration["Storage:InternalStorageUrl"];
                var externalUrl = _configuration["Storage:ExternalStorageUrl"];

                if (!string.IsNullOrWhiteSpace(internalUrl) && !string.IsNullOrWhiteSpace(externalUrl))
                {
                    uploadUrl = uploadUrl.Replace(internalUrl, externalUrl);
                    finalUrl = finalUrl.Replace(internalUrl, externalUrl);
                }
            }

            tickets.Add(new UploadTicketDto(uploadUrl, finalUrl));
        }

        return tickets;
    }

    public async Task InitializeCorsRulesAsync(CancellationToken cancellationToken)
    {
        var properties = await _blobServiceClient.GetPropertiesAsync(cancellationToken);

        // 1. Strip hidden Docker \r characters
        var configuredOrigin = _configuration["Storage:CorsAllowedOrigin"]?.Trim();

        var corsRule = new BlobCorsRule
        {
            MaxAgeInSeconds = 3600,

            // 2. EXPLICIT: No spaces after commas. Azurite string-matching is brutal.
            AllowedMethods = "GET,PUT,OPTIONS",

            // 3. EXPLICIT: We CANNOT use "*" here. We must spoon-feed Azurite the exact custom headers.
            AllowedHeaders = "content-type,x-ms-blob-type,accept,origin",

            ExposedHeaders = "*",

            // 4. Injects https://cpath-community-3aw1.vercel.app cleanly
            AllowedOrigins = string.IsNullOrEmpty(configuredOrigin) ? "*" : configuredOrigin
        };

        properties.Value.Cors.Clear();
        properties.Value.Cors.Add(corsRule);

        await _blobServiceClient.SetPropertiesAsync(properties.Value, cancellationToken);
    }
}