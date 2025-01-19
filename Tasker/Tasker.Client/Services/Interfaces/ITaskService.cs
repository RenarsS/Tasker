using Task = Tasker.API.Task;

namespace Tasker.Client.Services.Interfaces;

public interface ITaskService
{
    Task<Task> CreateTask(Task task);
}