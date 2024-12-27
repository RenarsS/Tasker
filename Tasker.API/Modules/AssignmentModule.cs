using Carter;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Routes;
using Tasker.Domain.DTO;

namespace Tasker.API.Modules;

public class AssignmentModule : CarterModule
{
    public AssignmentModule() : base("/api")
    {
        IncludeInOpenApi();
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(AssignmentRoutes.Base,
            async (IAssignmentService assignmentService) => await assignmentService.GetAssignments());
        
        app.MapGet(AssignmentRoutes.ById,
            async (int id, IAssignmentService assignmentService) => await assignmentService.GetAssignmentById(id));
        
        app.MapGet(AssignmentRoutes.ByTask,
            async (int taskId, IAssignmentService assignmentService) => await assignmentService.GetAssignmentsByTaskId(taskId));
        
        app.MapGet(AssignmentRoutes.ByUser,
            async (int userId, IAssignmentService assignmentService) => await assignmentService.GetAssignmentsByUserId(userId));
        
        app.MapPost(AssignmentRoutes.Base,
            async (Assignment assignment, IAssignmentService assignmentService) => await assignmentService.CreateAssignment(assignment));
        
        app.MapPut(AssignmentRoutes.Base,
            async (Assignment assignment, IAssignmentService assignmentService) => await assignmentService.UpdateAssignment(assignment));
    }
}