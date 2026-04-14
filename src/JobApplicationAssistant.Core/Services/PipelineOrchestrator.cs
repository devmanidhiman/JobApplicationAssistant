using System;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class PipelineOrchestrator: IPipelineOrchestrator
{
    private readonly ILogger<PipelineOrchestrator> _logger;
    private readonly ISkillExtractionService _skillExtractionService;

    public PipelineOrchestrator (ISkillExtractionService skillExtractionService, ILogger<PipelineOrchestrator> logger)
    {
        _skillExtractionService = skillExtractionService;
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

        // Steps 2, 3 and 4 will be added here

        return result;
    }

}
