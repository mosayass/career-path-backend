namespace CareerPath.Community.Core.DTOs;

public record UploadTicketDto(
    string UploadUrl, // The frontend uses this to execute the PUT request
    string FinalUrl   // The frontend saves this and sends it back to CreatePostCommand
);