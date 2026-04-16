namespace JobApplicationAssistant.Infrastructure.Common;

public static class JsonHelper
{
    public static string StripMarkdownFences(string response)
    {
        var trimmed = response.Trim();
        if (trimmed.StartsWith("```"))
        {
            var firstNewLine = trimmed.IndexOf('\n');
            if (firstNewLine != -1)
                trimmed = trimmed[(firstNewLine + 1)..];

            if (trimmed.EndsWith("```"))
                trimmed = trimmed[..^3];
        }
        return trimmed.Trim();
    }
}
