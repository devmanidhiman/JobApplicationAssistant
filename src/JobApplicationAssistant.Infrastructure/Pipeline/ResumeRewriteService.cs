using System.Runtime.CompilerServices;
using System.Text.Json;
using JobApplicationAssistant.Core.Interfaces;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Infrastructure.Common;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Infrastructure.Pipeline;

public class ResumeRewriteService : IResumeRewriteService
{
    private readonly IClaudeService _claudeService;
    private readonly ILogger<ResumeRewriteService> _logger;

    public ResumeRewriteService(IClaudeService claudeService, ILogger<ResumeRewriteService> logger)
    {
        _claudeService = claudeService;
        _logger = logger;   
    }

    public async Task<ResumeRewriteResult> RewriteAsync(SkillExtractionResult extractedSkills, 
                                                        string resumeText, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting resume rewrite");

        var systemPrompt = """
            You are an expert resume writer specializing in tailoring resumes to job descriptions.
            Your job is to rewrite resume bullet points to mirror the language and keywords of the target job.
            You must respond with valid JSON only — no preamble, no explanation, no markdown code fences.
            Use exactly the field names specified — do not rename, abbreviate, or alter any field name.
            The JSON must match this exact structure:
            {
                "rewrittenBullets": [
                    {
                        "original": "string",
                        "rewritten": "string",
                        "reasoning": "string"
                    }
                ],
                "summary": "string"
            }
            Rules:
            - Rewrite each bullet to naturally incorporate relevant keywords from the job description
            - Preserve the candidate's actual experience — do not fabricate or exaggerate
            - Do not add skills, tools, or experience the candidate has not mentioned
            - Do not change the meaning or scope of the original bullet — only adapt the language and phrasing
            - Keep bullets concise and action-oriented
            - summary is 2-3 sentences describing the overall rewrite strategy used
            """;
        
        var userMessage = $"""
            Target job keywords and skills: {JsonSerializer.Serialize(extractedSkills.RequiredSkills)}
            Nice to have skills: {JsonSerializer.Serialize(extractedSkills.NiceToHaveSkills)}
            Keywords: {JsonSerializer.Serialize(extractedSkills.Keywords)}

            Resume bullets to rewrite (one per line):
            {resumeText}
            """;

        var rawResponse = await _claudeService.CompleteAsync(systemPrompt, userMessage, cancellationToken);
        _logger.LogInformation("Raw Claude response: {Response}", rawResponse);
        var cleanedResponse = JsonHelper.StripMarkdownFences(rawResponse);
        try
        {
            var response = JsonSerializer.Deserialize<ResumeRewriteResult>(cleanedResponse, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            return response ?? throw new InvalidOperationException("Deserialization returned null"); 
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize Claude response: {Response}", rawResponse);
            throw new InvalidOperationException($"Claude returned invalid JSON: {rawResponse}", ex);
        }
    }
}
