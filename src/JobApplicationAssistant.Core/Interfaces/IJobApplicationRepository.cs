using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core.Interfaces;

public interface IJobApplicationRepository
{
    Task SavePipelineRunAsync(PipelineRequest request, PipelineResult result, CancellationToken cancellationToken = default);
    Task<Guid> SavePipelineStartAsync(PipelineRequest request, CancellationToken cancellationToken = default);
    Task SavePipelineErrorAsync(Guid jobApplicationId, string failedStep, string errorMessage, CancellationToken cancellationToken = default);
    Task UpdatePipelineStatusAsync(Guid jobApplicationId, string status, CancellationToken cancellationToken = default);
}