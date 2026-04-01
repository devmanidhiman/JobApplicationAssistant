using System;

namespace JobApplicationAssistant.Core.Interfaces;

public interface IClaudeService
{
    Task<string> CompleteAsync(string systemPrompt, string userMessage, CancellationToken cancellationToken = default);
}
