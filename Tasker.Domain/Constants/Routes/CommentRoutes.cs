namespace Tasker.Domain.Constants.Routes;

public static class CommentRoutes
{
    public const string Base = "/comments";

    public const string ById = Base + "/{id}";

    public const string ByTask = TaskRoutes.ById + Base;
    
    public const string Embed = Base + "-embed";
}