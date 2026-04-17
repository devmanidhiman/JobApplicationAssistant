using JobApplicationAssistant.Core.Models.Pipeline;

namespace JobApplicationAssistant.Core.Interfaces;

public interface ICoverLetterService
{
    Task<CoverLetterResult> GenerateCoverLetterAsync(SkillExtractionResult extractedSkills, 
                                        ResumeMatchResult resumeMatch, 
                                        string resumeText, 
                                        CancellationToken cancellationToken =default);
}
