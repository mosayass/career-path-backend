using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Assessment.Infrastructure.Persistence;
using CareerPath.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CareerPath.Assessment.Infrastructure.Repositories;

public class AssessmentRepository : IAssessmentRepository
{
    private readonly AssessmentsDbContext _dbContext;

    public AssessmentRepository(AssessmentsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid>> AddSubmissionAsync(AssessmentSubmission submission, CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.AssessmentSubmissions.AddAsync(submission, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(submission.Id);
        }
        catch (Exception ex)
        {
            // Translates database-level crashes into a clean Result failure
            return Result<Guid>.Failure($"Database error while saving assessment: {ex.Message}");
        }
    }
    

    public async Task<Result<AssessmentSubmission>> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var submission = await _dbContext.AssessmentSubmissions
                .Include(s => s.Result) // Eager load the AI predictions!
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

            if (submission == null)
                return Result<AssessmentSubmission>.Failure("Assessment not found.");

            return Result<AssessmentSubmission>.Success(submission);
        }
        catch (Exception ex)
        {
            return Result<AssessmentSubmission>.Failure($"Database error: {ex.Message}");
        }
    }
}