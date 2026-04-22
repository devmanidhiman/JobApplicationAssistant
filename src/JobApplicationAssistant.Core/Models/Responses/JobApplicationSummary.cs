namespace JobApplicationAssistant.Core.Models.Responses;

public class JobApplicationSummary
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string JobDescription { get; set; } = string.Empty;
}