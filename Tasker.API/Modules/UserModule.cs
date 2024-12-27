using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;

namespace Tasker.API.Modules;

public class UserModule : CarterModule
{
    public UserModule() : base("/api")
    {
        IncludeInOpenApi();
    }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(UserRoutes.Base, async (IUserService userService) => await userService.GetAllUsers());
        app.MapGet(UserRoutes.ById, async (int id, IUserService userService) => await userService.GetUserById(id));
    }
}