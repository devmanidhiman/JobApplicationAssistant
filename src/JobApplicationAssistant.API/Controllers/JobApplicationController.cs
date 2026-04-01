using Microsoft.AspNetCore.Http;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationAssistant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationController : ControllerBase
{
    private readonly IClaudeService _claudeService;

    public JobApplicationController(IClaudeService claudeService)
    {
        _claudeService = claudeService;
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
}

public record PingRequest(string Message);
