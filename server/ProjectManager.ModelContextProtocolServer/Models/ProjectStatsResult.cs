namespace ProjectManager.ModelContextProtocolServer.Models;

public class ProjectStatsResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ProjectStats? Stats { get; set; }
}