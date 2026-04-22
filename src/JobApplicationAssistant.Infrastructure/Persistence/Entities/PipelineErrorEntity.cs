namespace JobApplicationAssistant.Infrastructure.Persistence.Entities;

public class PipelineErrorEntity
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public string FailedStep { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public JobApplicationEntity JobApplication { get; set; } = null!;
}