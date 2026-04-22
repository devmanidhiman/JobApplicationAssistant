using System;

namespace JobApplicationAssistant.Infrastructure.Persistence.Entities;

public class PipelineResultEntity
{
    public Guid Id { get; set; }
    public Guid JobApplicationId { get; set; }
    public string SkillExtraction { get; set; } = string.Empty;
    public string ResumeMatch { get; set; } = string.Empty;
    public string ResumeRewrite { get; set; } = string.Empty;
    public string CoverLetter { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public JobApplicationEntity JobApplication { get; set; } = null!;
}
