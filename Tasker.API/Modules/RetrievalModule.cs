using Carter;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Modules;

public class RetrievalModule : CarterModule
{
    public RetrievalModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(RetrievalRoutes.Base, async ([FromBody]Task task, IRetrievalService retrievalService) => await retrievalService.GetRelevantOrders(task));
    }
}