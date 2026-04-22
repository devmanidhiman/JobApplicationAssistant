using System;
using System.Xml;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class PipelineOrchestrator: IPipelineOrchestrator
{
    private readonly ILogger<PipelineOrchestrator> _logger;
    private readonly ISkillExtractionService _skillExtractionService;
    private readonly IResumeMatchService _resumeMatchService;
    private readonly IResumeRewriteService _resumeRewriteService;
    private readonly ICoverLetterService _coverLetterService;
    private readonly IJobApplicationRepository _jobApplicationRepository;

    public PipelineOrchestrator (ISkillExtractionService skillExtractionService, 
                                IResumeMatchService resumeMatchService,
                                IResumeRewriteService resumeRewriteService,
                                ICoverLetterService coverLetterService, 
                                IJobApplicationRepository jobApplicationRepository,
                                ILogger<PipelineOrchestrator> logger)
    {
        _skillExtractionService = skillExtractionService;
        _resumeMatchService = resumeMatchService;
        _resumeRewriteService = resumeRewriteService;
        _coverLetterService = coverLetterService;
        _jobApplicationRepository = jobApplicationRepository;
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
        result.ResumeMatch = await _resumeMatchService.MatchAsync(
                                    result.SkillExtraction, 
                                    request.ResumeText, cancellationToken);
        

        _logger.LogInformation("Step 2 complete. Match score: {Score}, Missing skills: {Count}",
            result.ResumeMatch.MatchScore,
            result.ResumeMatch.MissingSkills.Count);

        //Step 3: Resume Rewriting
        result.ResumeRewrite = await _resumeRewriteService.RewriteAsync(
                                        result.SkillExtraction,
                                        request.ResumeText,
                                        cancellationToken);
        
        _logger.LogInformation("Step 3 complete. Rewritten bullets: {Count}",
            result.ResumeRewrite.RewrittenBullets.Count);

        //Step 4: Cover Letter Generation
        result.CoverLetter = await _coverLetterService.GenerateCoverLetterAsync(
                                        result.SkillExtraction,
                                        result.ResumeMatch, 
                                        request.ResumeText,
                                        cancellationToken);

        _logger.LogInformation("Step 4 complete. Cover letter length: {Length} chars",
            result.CoverLetter.CoverLetter.Length);

        _logger.LogInformation("Pipeline complete");

        await _jobApplicationRepository.SavePipelineRunAsync(request, result, cancellationToken);

        return result;
    }

}
