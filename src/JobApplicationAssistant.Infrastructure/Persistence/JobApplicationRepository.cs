using System.Text.Json;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Core.Models.Responses;
using JobApplicationAssistant.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Infrastructure.Persistence;

public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<JobApplicationRepository> _logger;

    public JobApplicationRepository(AppDbContext context, ILogger<JobApplicationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SavePipelineRunAsync (Guid jobApplicationId, PipelineResult result, 
                                            CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving Pipeline run to database.");

        var pipelineResult = new PipelineResultEntity
        {
            Id = Guid.NewGuid(),
            JobApplicationId = jobApplicationId,
            SkillExtraction = JsonSerializer.Serialize(result.SkillExtraction),
            ResumeMatch = JsonSerializer.Serialize(result.ResumeMatch),
            ResumeRewrite = JsonSerializer.Serialize(result.ResumeRewrite),
            CoverLetter = JsonSerializer.Serialize(result.CoverLetter),
            CreatedAt = DateTime.UtcNow  
        };

        await _context.PipelineResults.AddAsync(pipelineResult, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pipeline run saved. JobApplicationId: {Id}", jobApplicationId);

    }

    public async Task<Guid> SavePipelineStartAsync(PipelineRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving pipeline start to database");

        var jobApplication = new JobApplicationEntity
        {
            Id = Guid.NewGuid(),
            JobDescription = request.JobDescription,
            ResumeText = request.ResumeText,
            Status = "processing",
            CreatedAt = DateTime.UtcNow
        };

        await _context.JobApplications.AddAsync(jobApplication, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pipeline start saved. JobApplicationId: {Id}", jobApplication.Id);

        return jobApplication.Id;
    }

    public async Task SavePipelineErrorAsync(Guid jobApplicationId, string failedStep, string errorMessage, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving pipeline error for JobApplicationId: {Id}", jobApplicationId);

        var error = new PipelineErrorEntity
        {
            Id = Guid.NewGuid(),
            JobApplicationId = jobApplicationId,
            FailedStep = failedStep,
            ErrorMessage = errorMessage,
            CreatedAt = DateTime.UtcNow
        };

        await _context.PipelineErrors.AddAsync(error, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePipelineStatusAsync(Guid jobApplicationId, string status, CancellationToken cancellationToken = default)
    {
        var jobApplication = await _context.JobApplications
            .FirstOrDefaultAsync(j => j.Id == jobApplicationId, cancellationToken);

        if (jobApplication is null)
        {
            _logger.LogWarning("JobApplication not found for Id: {Id}", jobApplicationId);
            return;
        }

        jobApplication.Status = status;
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pipeline status updated to '{Status}' for JobApplicationId: {Id}", status, jobApplicationId);
    }

    public async Task<List<JobApplicationSummary>> GetAllAsync (CancellationToken cancellationToken)
    {
        _logger.LogInformation("Retrieving all job applications");

        return await _context.JobApplications
            .OrderByDescending(j => j.CreatedAt)
            .Select(j => new JobApplicationSummary
            {
                Id = j.Id,
                Status = j.Status,
                CreatedAt = j.CreatedAt,
                JobDescription = j.JobDescription
            }).ToListAsync();
    }

    public async Task<JobApplicationDetail?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving Job application by Id: {Id}", id);

        var jobApplication = await _context.JobApplications
            .FirstOrDefaultAsync(j => j.Id == id);

        if (jobApplication is null)
        {
            return null;
        }

        var pipelineResult = await _context.PipelineResults
            .FirstOrDefaultAsync(p => p.JobApplicationId == id, cancellationToken);

        _logger.LogInformation("PipelineResult found: {Found} for JobApplicationId: {Id}", pipelineResult is not null, id);

        var options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
        
        return new JobApplicationDetail
        {
            Id = jobApplication.Id,
            Status = jobApplication.Status,
            CreatedAt = jobApplication.CreatedAt,
            JobDescription = jobApplication.JobDescription,
            ResumeText = jobApplication.ResumeText,
            SkillExtraction = pipelineResult is null ? null : JsonSerializer.Deserialize<SkillExtractionResult>(pipelineResult.SkillExtraction, options),
            ResumeMatch = pipelineResult is null ? null : JsonSerializer.Deserialize<ResumeMatchResult>(pipelineResult.ResumeMatch, options),
            ResumeRewrite = pipelineResult is null ? null : JsonSerializer.Deserialize<ResumeRewriteResult>(pipelineResult.ResumeRewrite, options),
            CoverLetter = pipelineResult is null ? null : JsonSerializer.Deserialize<CoverLetterResult>(pipelineResult.CoverLetter, options)
        };
    }
}
