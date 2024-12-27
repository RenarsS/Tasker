using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.DTO;

namespace Tasker.API.Modules;

public class CommentModule : CarterModule
{
    public CommentModule() : base("/api")
    {
        IncludeInOpenApi();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(CommentRoutes.Base, async (ICommentService commentService) => await commentService.GetAllComments());
        app.MapGet(CommentRoutes.ById, async (int id, ICommentService commentService) => await commentService.GetCommentById(id));
        app.MapGet(CommentRoutes.ByTask, async (int taskId, ICommentService commentService) => await commentService.GetCommentsByTaskId(taskId));
        app.MapPost(CommentRoutes.Base, async (Comment comment, ICommentService commentService) => await commentService.CreateComment(comment));
        app.MapPut(CommentRoutes.Base, async (Comment comment, ICommentService commentService) => await commentService.UpdateComment(comment));
        app.MapDelete(CommentRoutes.ById, async (int id, ICommentService commentService) => await commentService.DeleteTask(id));
    }
}