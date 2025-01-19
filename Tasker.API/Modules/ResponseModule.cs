using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;

namespace Tasker.API.Modules;

public class ResponseModule : CarterModule
{
    public ResponseModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ResponseRoutes.Base, async (Response response, IResponseService responseService) => await responseService.CreateResponse(response));
        app.MapPost(ResponseRoutes.Rating, async (ResponseRetrievalRating responseRetrievalRating, IResponseService responseService) => await responseService.CreateResponseRetrievalRating(responseRetrievalRating));
    }
}