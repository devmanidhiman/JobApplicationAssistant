namespace JobApplicationAssistant.Core.Models.Pipeline;

public class PipelineResult
{
    public SkillExtractionResult? SkillExtraction { get; set; }
    public ResumeMatchResult? ResumeMatch {get; set;}
    public ResumeRewriteResult? ResumeRewrite {get; set;}
}
