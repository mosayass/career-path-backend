using System;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Core.Contracts;

public interface IAssessmentRepository
{
    // We return Result<Guid> strictly adhering to your error handling pattern
    Task<Result<Guid>> AddSubmissionAsync(AssessmentSubmission submission, CancellationToken cancellationToken = default);
    Task<Result<AssessmentSubmission>> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
}