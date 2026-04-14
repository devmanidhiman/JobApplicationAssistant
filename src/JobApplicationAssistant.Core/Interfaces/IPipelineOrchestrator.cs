
using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core;

public interface IPipelineOrchestrator
{
    Task<PipelineResult> RunAsync (PipelineRequest request, CancellationToken cancellationToken = default);

}
