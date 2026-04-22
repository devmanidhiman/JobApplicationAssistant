using System;
using System.Data.Common;
using System.Text.Json;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Infrastructure.Persistence.Entities;
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

    public async Task SavePipelineRunAsync (PipelineRequest request, PipelineResult result, 
                                            CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Saving Pipeline run to database.");
        var jobApplication = new JobApplicationEntity
        {
            Id = Guid.NewGuid(),
            JobDescription = request.JobDescription,
            ResumeText = request.ResumeText,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow  
        };

        var pipelineResult = new PipelineResultEntity
        {
            Id = Guid.NewGuid(),
            JobApplicationId = jobApplication.Id,
            SkillExtraction = JsonSerializer.Serialize(result.SkillExtraction),
            ResumeMatch = JsonSerializer.Serialize(result.ResumeMatch),
            ResumeRewrite = JsonSerializer.Serialize(result.ResumeRewrite),
            CoverLetter = JsonSerializer.Serialize(result.CoverLetter),
            CreatedAt = DateTime.UtcNow  
        };

        await _context.JobApplications.AddAsync(jobApplication, cancellationToken);
        await _context.PipelineResults.AddAsync(pipelineResult, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pipeline run saved. JobApplicationId: {Id}", jobApplication.Id);

    }
}
