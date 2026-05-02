using CareerPath.Community.Core.DTOs;
using FluentValidation;

namespace CareerPath.Community.Core.Features.Queries.GetUploadTickets;

public class GetUploadTicketsQueryValidator : AbstractValidator<GetUploadTicketsQuery>
{
    public GetUploadTicketsQueryValidator()
    {
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage("At least one file request must be provided.");

        RuleForEach(x => x.Files).SetValidator(new MediaUploadRequestDtoValidator());
    }
}

// Sub-validator for the individual items in the list
public class MediaUploadRequestDtoValidator : AbstractValidator<MediaUploadRequestDto>
{
    public MediaUploadRequestDtoValidator()
    {
        RuleFor(x => x.FileName).NotEmpty().WithMessage("File name is required.");
        RuleFor(x => x.ContentType).NotEmpty().WithMessage("Content type is required.");
    }
}