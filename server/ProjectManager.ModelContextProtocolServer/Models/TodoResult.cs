namespace ProjectManager.ModelContextProtocolServer.Models;

public class TodoResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public TodoItem? TodoItem { get; set; }
}