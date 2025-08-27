using ModelContextProtocol.Server;
using ProjectManager.ModelContextProtocolServer.Models;
using ProjectManager.ModelContextProtocolServer.Service;

namespace ProjectManager.ModelContextProtocolServer.Tools;

[McpServerToolType]
public class ProjectTool
{
    private readonly IProjectService _projectService;

    public ProjectTool(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [McpServerTool(
        Title = "�� ������Ʈ ����",
        ReadOnly = false,
        Destructive = false,
        Idempotent = false,
        OpenWorld = false)]
    public ProjectCreateResult CreateProject(string name, string description)
    {   
        return _projectService.CreateProject(name, description);
    }

    [McpServerTool(
        Title = "��� ������Ʈ ��� ��ȸ",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectListResult GetAllProjects()
    {
        return _projectService.GetAllProjects();
    }

    [McpServerTool(
        Title = "Ư�� ������Ʈ �� ���� ��ȸ",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult GetProject(string projectId)
    {
        return _projectService.GetProject(projectId);
    }

    [McpServerTool(
        Title = "������Ʈ ���� ����",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult UpdateProject(string projectId, string? name = null, string? description = null)
    {
        return _projectService.UpdateProject(projectId, name, description);
    }

    [McpServerTool(
        Title = "������Ʈ ����",
        ReadOnly = false,
        Destructive = true,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult DeleteProject(string projectId)
    {
        return _projectService.DeleteProject(projectId);
    }

    [McpServerTool(
        Title = "������Ʈ�� �� ���� �߰�",
        ReadOnly = false,
        Destructive = false,
        Idempotent = false,
        OpenWorld = false)]
    public TodoResult AddTodoItem(string projectId, string title, string description = "")
    {
        return _projectService.AddTodoItem(projectId, title, description);
    }

    [McpServerTool(
        Title = "���� �Ϸ� ó��",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public TodoResult CompleteTodoItem(string projectId, string todoId)
    {
        return _projectService.CompleteTodoItem(projectId, todoId);
    }

    [McpServerTool(
        Title = "���� ����",
        ReadOnly = false,
        Destructive = true,
        Idempotent = true,
        OpenWorld = false)]
    public TodoResult DeleteTodoItem(string projectId, string todoId)
    {
        return _projectService.DeleteTodoItem(projectId, todoId);
    }

    [McpServerTool(
        Title = "������Ʈ ��ü ��� ��ȸ",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectStatsResult GetProjectStats()
    {
        return _projectService.GetProjectStats();
    }
}
