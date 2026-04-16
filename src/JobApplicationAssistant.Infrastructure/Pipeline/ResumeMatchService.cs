using System;
using System.Text.Json;
using JobApplicationAssistant.Core;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Infrastructure.Pipeline;

public class ResumeMatchService : IResumeMatchService
{
    private readonly ILogger<ResumeMatchService> _logger;
    private readonly IClaudeService _claudeService;

    public ResumeMatchService(ILogger<ResumeMatchService> logger, IClaudeService claudeService)
    {
        _logger = logger;
        _claudeService = claudeService;
    }

    public async Task<ResumeMatchResult> MatchAsync(SkillExtractionResult extractedSkills, string resumeText, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting Resume Match.");

        var systemPrompt = """
            You are a precise resume analyzer.
            You compare a candidate's resume against a list of required and nice-to-have skills.
            You must respond with valid JSON only — no preamble, no explanation, no markdown code fences.
            Use exactly the field names specified — do not rename, abbreviate, or alter any field name.
            The JSON must match this exact structure:
            {
                "matchScore": 0,
                "matchedSkills": ["string"],
                "missingSkills": ["string"],
                "partialMatches": ["string"],
                "summary": "string"
            }
            matchScore is an integer from 0 to 100 reflecting how well the resume matches the required skills.
            partialMatches are skills the candidate has some but not full experience with.
            summary is 2-3 sentences explaining the score.
            """;

        var userMessage = $"""
            Required skills: {JsonSerializer.Serialize(extractedSkills.RequiredSkills)}
            Nice to have skills: {JsonSerializer.Serialize(extractedSkills.NiceToHaveSkills)}
            
            Candidate resume:
            {resumeText}
            """;

        var rawResponse = await _claudeService.CompleteAsync(systemPrompt, userMessage, cancellationToken);

        _logger.LogInformation("Raw Claude response: {Response}", rawResponse);

        try
        {
            var result = JsonSerializer.Deserialize<ResumeMatchResult>(rawResponse, new JsonSerializerOptions
            {
               PropertyNameCaseInsensitive = true 
            });
            return result ?? throw new InvalidOperationException("Deserialization returned null");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize Claude response: {Response}", rawResponse);
            throw new InvalidOperationException($"Claude returned invalid JSON: {rawResponse}", ex);
        }

    }

}
