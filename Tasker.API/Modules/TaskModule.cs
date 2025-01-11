using Carter;
using Tasker.API.Jobs;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Modules;

public class TaskModule : CarterModule
{
    public TaskModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(TaskRoutes.Base, async (ITaskService taskService) => await taskService.GetAllTasks());
        app.MapGet(TaskRoutes.ById, async (int id, ITaskService taskService) => await taskService.GetTaskById(id));
        app.MapPost(TaskRoutes.Base, async (Task task, ITaskService taskService) => await taskService.CreateTask(task));
        app.MapPut(TaskRoutes.Base, async (Task task, ITaskService taskService ) => await taskService.UpdateTask(task));
        app.MapDelete(TaskRoutes.ById, async (int id, ITaskService taskService) => await taskService.DeleteTask(id));
        app.MapPost(TaskRoutes.Embed, async (ITaskService taskService) => await taskService.EmbedTasks());
    }
}