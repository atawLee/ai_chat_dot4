namespace ProjectManager.ModelContextProtocolServer.Models;

public class ProjectResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Project? Project { get; set; }
}