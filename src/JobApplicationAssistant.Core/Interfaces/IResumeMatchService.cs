namespace JobApplicationAssistant.Core.Models.Pipeline;

public interface IResumeMatchService
{
    Task<ResumeMatchResult> MatchAsync(SkillExtractionResult extractedSkills, string resumeText, CancellationToken cancellationToken = default);
}
