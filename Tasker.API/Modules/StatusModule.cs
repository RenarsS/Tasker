using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;

namespace Tasker.API.Modules;

public class StatusModule : CarterModule
{
    public StatusModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(StatusRoutes.Base, async (IStatusService statusService) => await statusService.GetAllStatuses());
        app.MapGet(StatusRoutes.ById, async (int id, IStatusService statusService) => await statusService.GetStatusById(id));
    }
}