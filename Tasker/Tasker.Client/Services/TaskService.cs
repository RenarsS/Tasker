using Tasker.API;
using Tasker.Client.Services.Interfaces;
using Task = Tasker.API.Task;

namespace Tasker.Client.Services;

public class TaskService(ITaskerClient taskerClient) : ITaskService
{
    public async Task<Task> CreateTask(Task task)
        =>  await taskerClient.PostApiTasksAsync(task);
}