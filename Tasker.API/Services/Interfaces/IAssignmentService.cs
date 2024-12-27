using Tasker.Domain.DTO;

namespace Tasker.API.Services.Interfaces;

public interface IAssignmentService
{
    Task<IEnumerable<Assignment>> GetAssignments();

    Task<Assignment> GetAssignmentById(int id);
    
    Task<IEnumerable<Assignment>> GetAssignmentsByTaskId(int taskId);

    Task<IEnumerable<Assignment>> GetAssignmentsByUserId(int userId);

    Task<Assignment> CreateAssignment(Assignment assignment);

    Task<Assignment> UpdateAssignment(Assignment assignment);

}