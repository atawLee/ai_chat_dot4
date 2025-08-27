namespace ProjectManager.ModelContextProtocolServer.Models;

public class ProjectListResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<Project> Projects { get; set; } = new();
}