using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;

namespace Tasker.API.Modules;

public class TaskTypeModule : CarterModule
{
    public TaskTypeModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(TaskTypeRoutes.Base, async (ITaskTypeService taskTypeService) => await taskTypeService.GetAllTaskTypes());
        app.MapGet(TaskTypeRoutes.ById, async (int id, ITaskTypeService taskTypeService) => await taskTypeService.GetTaskTypeById(id));
    }
}