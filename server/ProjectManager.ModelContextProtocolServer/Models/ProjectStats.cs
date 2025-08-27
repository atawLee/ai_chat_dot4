namespace ProjectManager.ModelContextProtocolServer.Models;

public class ProjectStats
{
    public int TotalProjects { get; set; }
    public int TotalTodos { get; set; }
    public int CompletedTodos { get; set; }
    public double CompletionRate { get; set; }
}