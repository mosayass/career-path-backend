using CareerPath.Assessment.Core.Contracts;
using CareerPath.Assessment.Core.Entities;
using CareerPath.Assessment.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace CareerPath.Assessment.Infrastructure.Repositories;

public class AssessmentRepository(AssessmentsDbContext dbContext) : IAssessmentRepository
{
    private readonly AssessmentsDbContext _dbContext = dbContext;

    public async Task<Guid> AddSubmissionAsync(AssessmentSubmission submission, CancellationToken cancellationToken = default)
    {
        await _dbContext.AssessmentSubmissions.AddAsync(submission, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return submission.Id;
    }


    public async Task<AssessmentSubmission?> GetSubmissionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AssessmentSubmissions
            .Include(s => s.Result) // Eager load the AI predictions!
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}