using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core.Interfaces;

public interface ISkillExtractionService
{
    Task<SkillExtractionResult> ExtractSkillsAsync(string jobDescription, CancellationToken cancellationToken = default);
}
