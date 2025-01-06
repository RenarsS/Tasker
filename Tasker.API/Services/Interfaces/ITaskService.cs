using Tasker.Domain.Import;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services.Interfaces;

public interface ITaskService
{
    public Task<IEnumerable<Task>> GetAllTasks();
    
    public Task<Task> GetTaskById(int id);
    
    public Task<Task?> CreateTask(Task task);
    
    public Task<Task> UpdateTask(Task task);
    
    public System.Threading.Tasks.Task DeleteTask(int id);

    Task<DataBatch> GetTaskDataBatch(int id);
    
    Task<Task> GetTaskByVectorId (string vectorId);

    Task<bool> EmbedTasks();
}