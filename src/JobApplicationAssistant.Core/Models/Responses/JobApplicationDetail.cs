using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core.Models.Responses;

public class JobApplicationDetail
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string JobDescription { get; set; } = string.Empty;
    public string ResumeText { get; set; } = string.Empty;
    public SkillExtractionResult? SkillExtraction { get; set; }
    public ResumeMatchResult? ResumeMatch { get; set; }
    public ResumeRewriteResult? ResumeRewrite { get; set; }
    public CoverLetterResult? CoverLetter { get; set; }
}