namespace JobApplicationAssistant.Core.Models.Pipeline;

public interface IResumeRewriteService
{
    Task<ResumeRewriteResult> RewriteAsync(SkillExtractionResult extractedSkills, 
                                        string resumeText, 
                                        CancellationToken cancellationToken = default);
}
