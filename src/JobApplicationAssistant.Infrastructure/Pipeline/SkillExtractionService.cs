using JobApplicationAssistant.Core.Interfaces;
using Microsoft.Extensions.Logging;
using JobApplicationAssistant.Core.Models.Pipeline;
using System.Text.Json;
using JobApplicationAssistant.Infrastructure.Common;

namespace JobApplicationAssistant.Infrastructure.Pipeline;

public class SkillExtractionService : ISkillExtractionService
{
    private readonly IClaudeService _claudeService;
    private readonly ILogger<SkillExtractionService> _logger;

    public SkillExtractionService(IClaudeService claudeService, ILogger<SkillExtractionService> logger)
    {
        _claudeService = claudeService;
        _logger = logger;
    }

    public async Task<SkillExtractionResult> ExtractSkillsAsync(string jobDescription, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting skill extraction.");

        var systemPrompt = """
            You are a precise job description analyzer.
            Your only job is to extract structured information from job descriptions.
            You must respond with valid JSON only — no preamble, no explanation, no markdown code fences.
            The JSON must match this exact structure:
            {
                "jobTitle": "string",
                "seniorityLevel": "string",
                "requiredSkills": ["string"],
                "niceToHaveSkills": ["string"],
                "keywords": ["string"]
            }
        """;

        var userMessage = $"""
            Extract skills and keywords from this job description:

            {jobDescription}
            """;

        var rawResponse = await _claudeService.CompleteAsync(systemPrompt, userMessage, cancellationToken);

        _logger.LogInformation("Raw Claude Response: {Response}", rawResponse);
        var cleanedResponse = JsonHelper.StripMarkdownFences(rawResponse);

        try
        {
            var result = JsonSerializer.Deserialize<SkillExtractionResult>(cleanedResponse, new JsonSerializerOptions
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
