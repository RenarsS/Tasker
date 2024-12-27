namespace Tasker.Domain.Constants.Routes;

public static class TaskRoutes
{
    public const string Base = "/tasks";

    public const string ById = Base + "/{id}";
    
    public const string Batches = Base + "-full";
    
    public const string BatchById = Batches + "/{id}";
}