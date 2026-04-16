using System;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class PipelineOrchestrator: IPipelineOrchestrator
{
    private readonly ILogger<PipelineOrchestrator> _logger;
    private readonly ISkillExtractionService _skillExtractionService;
    private readonly IResumeMatchService _resumeMatchService;

    public PipelineOrchestrator (ISkillExtractionService skillExtractionService, 
                                IResumeMatchService resumeMatchService, 
                                ILogger<PipelineOrchestrator> logger)
    {
        _skillExtractionService = skillExtractionService;
        _resumeMatchService = resumeMatchService;
        _logger = logger;
    }

    public async Task<PipelineResult> RunAsync(PipelineRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Pipeline started");
        var result = new PipelineResult();

        // Step 1: Extract skill
        result.SkillExtraction = await _skillExtractionService.ExtractSkillsAsync(
                request.JobDescription, cancellationToken);

        _logger.LogInformation("Step 1 complete. Job title: {Title}, Required skills: {Count}",
            result.SkillExtraction.JobTitle,
            result.SkillExtraction.RequiredSkills.Count);

        //Step 2: Resume Match
        result.ResumeMatch = await _resumeMatchService.MatchAsync(result.SkillExtraction, 
                                    request.ResumeText, cancellationToken);
        

        _logger.LogInformation("Step 2 complete. Match score: {Score}, Missing skills: {Count}",
            result.ResumeMatch.MatchScore,
            result.ResumeMatch.MissingSkills.Count);

        // Steps 3 and 4 will be added here

        return result;
    }

}
