namespace Tasker.Domain.Constants.Routes;

public static class AssignmentRoutes
{
    public const string Base = "/assignments";
    
    public const string ById = Base + "/{id}";

    public const string ByTask = TaskRoutes.ById + Base;

    public const string ByUser = UserRoutes.ById + Base;
}