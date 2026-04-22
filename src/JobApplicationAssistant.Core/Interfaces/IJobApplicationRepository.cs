using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core.Interfaces;

public interface IJobApplicationRepository
{
    Task SavePipelineRunAsync(PipelineRequest request, PipelineResult result, CancellationToken cancellationToken = default);
}