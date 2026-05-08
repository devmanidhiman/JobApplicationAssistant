using System;

namespace JobApplicationAssistant.Core.Models.Pipeline;

public class PipelineResponse
{
    public Guid JobApplicationId {get; set;}
    public PipelineResult Result {get; set;} = null!;
    
}
