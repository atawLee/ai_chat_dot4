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
        Title = "새 프로젝트 생성",
        ReadOnly = false,
        Destructive = false,
        Idempotent = false,
        OpenWorld = false)]
    public ProjectCreateResult CreateProject(string name, string description)
    {   
        return _projectService.CreateProject(name, description);
    }

    [McpServerTool(
        Title = "모든 프로젝트 목록 조회",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectListResult GetAllProjects()
    {
        return _projectService.GetAllProjects();
    }

    [McpServerTool(
        Title = "특정 프로젝트 상세 정보 조회",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult GetProject(string projectId)
    {
        return _projectService.GetProject(projectId);
    }

    [McpServerTool(
        Title = "프로젝트 정보 수정",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult UpdateProject(string projectId, string? name = null, string? description = null)
    {
        return _projectService.UpdateProject(projectId, name, description);
    }

    [McpServerTool(
        Title = "프로젝트 삭제",
        ReadOnly = false,
        Destructive = true,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectResult DeleteProject(string projectId)
    {
        return _projectService.DeleteProject(projectId);
    }

    [McpServerTool(
        Title = "프로젝트에 새 할일 추가",
        ReadOnly = false,
        Destructive = false,
        Idempotent = false,
        OpenWorld = false)]
    public TodoResult AddTodoItem(string projectId, string title, string description = "")
    {
        return _projectService.AddTodoItem(projectId, title, description);
    }

    [McpServerTool(
        Title = "할일 완료 처리",
        ReadOnly = false,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public TodoResult CompleteTodoItem(string projectId, string todoId)
    {
        return _projectService.CompleteTodoItem(projectId, todoId);
    }

    [McpServerTool(
        Title = "할일 삭제",
        ReadOnly = false,
        Destructive = true,
        Idempotent = true,
        OpenWorld = false)]
    public TodoResult DeleteTodoItem(string projectId, string todoId)
    {
        return _projectService.DeleteTodoItem(projectId, todoId);
    }

    [McpServerTool(
        Title = "프로젝트 전체 통계 조회",
        ReadOnly = true,
        Destructive = false,
        Idempotent = true,
        OpenWorld = false)]
    public ProjectStatsResult GetProjectStats()
    {
        return _projectService.GetProjectStats();
    }
}
