namespace JobApplicationAssistant.Core.Models.Pipeline;

public class SkillExtractionResult
{
    public List<string> RequiredSkills {get; set;} = [];
    public List<string> NiceToHaveSkills { get; set; } = [];
    public List<string> Keywords { get; set; } = [];
    public string JobTitle { get; set; } = string.Empty;
    public string SeniorityLevel { get; set; } = string.Empty;

}
