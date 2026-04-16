using System;
using System.Dynamic;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class ResumeMatchResult
{
    public int MatchScore {get; set;}
    public List<string> MatchedSkills {get; set;} = [];
    public List<string> MissingSkills {get; set;} = [];
    public List<string> PartialMatch {get; set;} = [];
    public string Summary {get; set;} = string.Empty;
}
