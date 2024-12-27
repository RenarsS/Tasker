using Carter;
using Tasker.API.Services;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.Import;
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
        app.MapGet(TaskRoutes.BatchById,
            async (int id, ITaskService taskService) => await taskService.GetTaskDataBatch(id));
        app.MapPost(TaskRoutes.Batches,
            async (IEnumerable<DataBatch> dataBatches, IDataImportService dataImportService) =>
                await dataImportService.ImportDataBatches(dataBatches));
    }
}