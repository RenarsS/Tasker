using Quartz;
using Tasker.API.Services.Interfaces;

namespace Tasker.API.Jobs;

public class TaskEmbeddingJob(ITaskService taskService) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        await taskService.EmbedTasks();
    }
}