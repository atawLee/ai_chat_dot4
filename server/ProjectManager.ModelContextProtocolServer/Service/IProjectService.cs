using ProjectManager.ModelContextProtocolServer.Models;

namespace ProjectManager.ModelContextProtocolServer.Service;

public interface IProjectService
{
    ProjectCreateResult CreateProject(string name, string description);
    ProjectListResult GetAllProjects();
    ProjectResult GetProject(string projectId);
    ProjectResult UpdateProject(string projectId, string? name = null, string? description = null);
    ProjectResult DeleteProject(string projectId);
    TodoResult AddTodoItem(string projectId, string title, string description = "");
    TodoResult CompleteTodoItem(string projectId, string todoId);
    TodoResult DeleteTodoItem(string projectId, string todoId);
    ProjectStatsResult GetProjectStats();
}
