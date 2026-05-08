
using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core;

public interface IPipelineOrchestrator
{
    Task<PipelineResponse> RunAsync (PipelineRequest request, CancellationToken cancellationToken = default);

}
