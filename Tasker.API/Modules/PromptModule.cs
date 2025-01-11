using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;

namespace Tasker.API.Modules;

public class PromptModule : CarterModule
{
    public PromptModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(PromptRoutes.Base, async (string promptName, IPromptService promptService) => await promptService.GetPrompt(promptName));
    }
}