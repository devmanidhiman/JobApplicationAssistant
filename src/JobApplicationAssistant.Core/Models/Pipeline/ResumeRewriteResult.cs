using System;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class ResumeRewriteResult
{
    public List<RewrittenBullet> RewrittenBullets {get; set;} = [];
    public string Summary {get; set;} = string.Empty; 
}

public class RewrittenBullet
{
    public string Original {get; set;} = string.Empty;
    public string Rewritten {get; set;} = string.Empty;
    public string reasoning {get; set;} = string.Empty;
}
