using System.Collections;
using Tasker.Domain.DTO.Analytics;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface ITaskRepository : IRepository
{
    Task<IEnumerable<Task>> GetTasks();
    
    Task<Task> GetTaskById(int id);
    
    Task<int> InsertTask(Task task);
    
    Task<Task> UpdateTask(Task task);
    
    System.Threading.Tasks.Task DeleteTask(int id);
    
    Task<Task> GetTaskByVectorId(string vectorId);

    Task<IEnumerable<Task>> GetTasksNotEmbedded();
    
    Task<IEnumerable<Task>> GetOrdersNotEmbedded();

    System.Threading.Tasks.Task InsertTaskRetrievalRating(TaskRetrievalRating taskRetrievalRating);
    
    System.Threading.Tasks.Task LinkToOrderVector(int taskId, string vectorId);
}