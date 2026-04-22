namespace JobApplicationAssistant.Infrastructure.Persistence.Entities;

public class JobApplicationEntity
{
    public Guid Id {get; set;}
    public DateTime CreatedAt {get; set;}
    public string JobDescription {get; set;} = string.Empty;
    public string ResumeText {get; set;} = string.Empty;
    public string Status {get; set;} = string.Empty;
}
