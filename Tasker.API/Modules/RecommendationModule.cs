using Carter;
using Microsoft.AspNetCore.Mvc;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Modules;

public class RecommendationModule : CarterModule
{
    public RecommendationModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(RecommendationRoutes.Base, async (Task task, int relevantTaskCount, IRecommendationService recommendationService) => await recommendationService.GenerateRecommendationResponse(task, relevantTaskCount));
    }
}