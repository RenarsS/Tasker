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
        app.MapPost(DataBatchRoutes.Base, async (DataBatch dataBatch, IDataImportService dataImportService) => await dataImportService.ImportDataBatch(dataBatch));
    }
}