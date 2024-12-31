using Tasker.Domain.DTO;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IAssignmentRepository : IRepository
{
    Task<IEnumerable<Assignment>> GetAssignments();
    
    Task<Assignment> GetAssignmentById(int id);
    
    Task<IEnumerable<Assignment>> GetAssignmentsByTaskId(int taskId);

    Task<IEnumerable<Assignment>> GetAssignmentsByUserId(int userId);
    
    Task<Assignment> InsertAssignment(Assignment comment);
    
    Task<Assignment> UpdateAssignment(Assignment comment);
}