using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.Import;

namespace Tasker.API.Modules;

public class DataImportModule : CarterModule
{
    public DataImportModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(DataBatchRoutes.Base,
            async (int id, ITaskService taskService) => await taskService.GetTaskDataBatch(id));
        app.MapPost(DataBatchRoutes.Base, 
            async (DataBatch dataBatch, IDataImportService dataImportService) => await dataImportService.ImportDataBatch(dataBatch));
        app.MapPost(DataBatchRoutes.Batches,
            async (List<DataBatch> dataBatches, IDataImportService dataImportService) =>
                await dataImportService.ImportDataBatches(dataBatches));
    }
}