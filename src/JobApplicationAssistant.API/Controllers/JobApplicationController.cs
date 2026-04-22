using JobApplicationAssistant.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JobApplicationAssistant.Core.Models.Pipeline;
using JobApplicationAssistant.Core;
using JobApplicationAssistant.Core.Models.Responses;

namespace JobApplicationAssistant.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationController : ControllerBase
{
    private readonly IClaudeService _claudeService;
    private readonly IPipelineOrchestrator _pipelineOrchestrator;
    private readonly IJobApplicationRepository _repository;

    public JobApplicationController(IClaudeService claudeService, 
                                    IPipelineOrchestrator pipelineOrchestrator,
                                    IJobApplicationRepository repository)
    {
        _claudeService = claudeService;
        _pipelineOrchestrator = pipelineOrchestrator;
        _repository = repository;
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

    [HttpGet]
    public async Task<ActionResult<List<JobApplicationSummary>>> GetAll(CancellationToken cancellationToken)
    {
        var results = await _repository.GetAllAsync(cancellationToken);
        return Ok(results);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobApplicationDetail>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByIdAsync(id, cancellationToken);
        if (result is null)
            return NotFound($"No job application found with id: {id}");

        return Ok(result);
    }

    
}

public record PingRequest(string Message);
