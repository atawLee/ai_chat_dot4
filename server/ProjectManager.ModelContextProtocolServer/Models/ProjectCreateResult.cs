namespace ProjectManager.ModelContextProtocolServer.Models;

public class ProjectCreateResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Project? Project { get; set; }
}