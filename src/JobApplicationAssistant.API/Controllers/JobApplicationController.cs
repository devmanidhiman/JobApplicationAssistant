using Microsoft.AspNetCore.Http;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Core;
using System.IO.Pipelines;

namespace JobApplicationAssistant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationController : ControllerBase
{
    private readonly IClaudeService _claudeService;
    private readonly IPipelineOrchestrator _pipelineOrchestrator;

    public JobApplicationController(IClaudeService claudeService, IPipelineOrchestrator pipelineOrchestrator)
    {
        _claudeService = claudeService;
        _pipelineOrchestrator = pipelineOrchestrator;
    }

    [HttpPost("ping")]
    public async Task<IActionResult> Ping([FromBody] PingRequest request, CancellationToken cancellationToken)
    {
        var response = await _claudeService.CompleteAsync(
            systemPrompt: "You are a helpful assistant for job applications.",
            userMessage: request.Message,
            cancellationToken: cancellationToken
        );

        return Ok(new { response });
    }

    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze([FromBody] PipelineRequest request, CancellationToken cancellationToken)
    {
        var response = await _pipelineOrchestrator.RunAsync(request, cancellationToken);
        return Ok(response);
    }
}

public record PingRequest(string Message);
