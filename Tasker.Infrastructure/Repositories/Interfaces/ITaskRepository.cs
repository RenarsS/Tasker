using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface ITaskRepository : IRepository
{
    Task<IEnumerable<Task>> GetTasks();
    
    Task<Task> GetTaskById(int id);
    
    Task<int> InsertTask(Task task);
    
    Task<Task> UpdateTask(Task task);
    
    System.Threading.Tasks.Task DeleteTask(int id);
}