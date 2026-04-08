using System;
using System.Threading;
using System.Threading.Tasks;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Shared.Responses;

namespace CareerPath.Assessment.Core.Contracts;

public interface IAssessmentRepository
{
    Task<Guid> AddSubmissionAsync(AssessmentSubmission submission, CancellationToken cancellationToken = default);
    Task<AssessmentSubmission?> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}