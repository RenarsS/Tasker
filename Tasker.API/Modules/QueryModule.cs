using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;

namespace Tasker.API.Modules;

public class QueryModule : CarterModule
{
    public QueryModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(QueryRoutes.Base, async (string prompt,Query query, IQueryService queryService) => await queryService.CreateQuery(prompt, query));
        app.MapPost(QueryRoutes.Rating, async (QueryResponseRating queryResponseRating, IQueryService queryService) => await queryService.CreateQueryResponseRating(queryResponseRating));
    }
}