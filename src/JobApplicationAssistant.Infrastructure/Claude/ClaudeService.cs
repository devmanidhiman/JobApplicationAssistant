using Anthropic;
using Anthropic.Models.Messages;
using JobApplicationAssistant.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace JobApplicationAssistant.Infrastructure.Claude;

public class ClaudeService : IClaudeService
{
    private readonly AnthropicClient _client;
    private readonly ILogger<ClaudeService> _logger;

    // Model constant — swap this one string to upgrade everywhere
    private const string Model = "claude-sonnet-4-6";
    private const int MaxTokens = 2048;

    public ClaudeService(AnthropicClient client, ILogger<ClaudeService> logger)
    {
        _client = client;
        _logger = logger;
    }
    public async Task<string> CompleteAsync(string systemPrompt, string userMessage, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Calling Claude API. Model: {Model}", Model);
        var parameters = new MessageCreateParams
        {
            Model = Model,
            MaxTokens = MaxTokens,
            System = systemPrompt,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = userMessage
                }
            ]
        };

        var response = await _client.Messages.Create(parameters, cancellationToken: cancellationToken);
        var text = response.Content
                    .Select( b => b.Value)
                    .OfType<TextBlock>()    
                    .FirstOrDefault()?.Text ?? string.Empty;

        var inputTokens = response.Usage.InputTokens;
        var outputTokens = response.Usage.OutputTokens;

        // Calculate cost in USD
        var inputCost = (inputTokens / 1_000_000m) * 3m;
        var outputCost = (outputTokens / 1_000_000m) * 15m;
        var totalCost = inputCost + outputCost;

        _logger.LogInformation(
        "Claude response received. Input tokens: {Input}, Output tokens: {Output}, Cost: ${Cost}",
        inputTokens,
        outputTokens,
        totalCost.ToString("F6")   // 6 decimal places e.g. $0.000142
        );

        return text;
    }
}
