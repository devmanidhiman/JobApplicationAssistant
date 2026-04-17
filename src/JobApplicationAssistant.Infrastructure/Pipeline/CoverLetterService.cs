using System;
using System.Text.Json;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Infrastructure.Pipeline;

public class CoverLetterService : ICoverLetterService
{
    private readonly IClaudeService _claudeService;
    private readonly ILogger<CoverLetterService> _logger;

    public CoverLetterService(IClaudeService claudeService, ILogger<CoverLetterService> logger)
    {
        _claudeService = claudeService;
        _logger = logger;
    }

    public async Task<CoverLetterResult> GenerateCoverLetterAsync(SkillExtractionResult extractedSkills, 
                                                                    ResumeMatchResult resumeMatch,
                                                                    string resumeText,
                                                                    CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting cover Letter generation");

        var systemPrompt = """
            You are an expert cover letter writer specializing in tech and fintech roles.
            Your job is to write a tailored, compelling cover letter based on the candidate's resume and the target job.
            You must respond with valid JSON only — no preamble, no explanation, no markdown code fences.
            Use exactly the field names specified — do not rename, abbreviate, or alter any field name.
            The JSON must match this exact structure:
            {
                "coverLetter": "string",
                "strategy": "string"
            }
            Rules:
            - Write a professional, natural cover letter of 3-4 paragraphs
            - Open with a strong hook that references the specific role and company type
            - Highlight matched skills naturally — do not list them mechanically
            - Acknowledge gaps honestly if relevant, framing them as growth areas
            - Do not fabricate experience or skills the candidate has not demonstrated
            - Do not use generic filler phrases like 'I am a passionate developer' or 'I am a quick learner'
            - strategy is 2-3 sentences explaining the tailoring approach used
            """;
        
        var userMessage = $"""
            Target job title: {extractedSkills.JobTitle}
            Seniority level: {extractedSkills.SeniorityLevel}
            Required skills: {JsonSerializer.Serialize(extractedSkills.RequiredSkills)}
            Nice to have skills: {JsonSerializer.Serialize(extractedSkills.NiceToHaveSkills)}
            Match score: {resumeMatch.MatchScore}/100
            Matched skills: {JsonSerializer.Serialize(resumeMatch.MatchedSkills)}
            Missing skills: {JsonSerializer.Serialize(resumeMatch.MissingSkills)}

            Candidate resume:
            {resumeText}
            """;

        var rawResponse = await _claudeService.CompleteAsync(systemPrompt, userMessage, cancellationToken);

        _logger.LogInformation("Raw Claude response: {Response}", rawResponse);

        try
        {
            var cleanedResponse = JsonHelper.StripMarkdownFences(rawResponse);
            var result = JsonSerializer.Deserialize<CoverLetterResult>(cleanedResponse, new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true
            });

            return result ?? throw new InvalidOperationException($"Deserialization returned null in {nameof(CoverLetterService)}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize Claude response: {Response}", rawResponse);
            throw new InvalidOperationException($"Claude returned invalid JSON. Error : {rawResponse}", ex);
        }
    }
}
