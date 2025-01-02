using Tasker.API.Services.Interfaces;
using Tasker.Domain.Import;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class TaskService(
    ITaskRepository taskRepository, 
    IAssignmentRepository assignmentRepository, 
    ICommentRepository  commentRepository,
    IEmbeddingProcessor embeddingProcessor) : ITaskService
{
    public async Task<IEnumerable<Task>> GetAllTasks()
    {
        var tasks = await taskRepository.GetTasks();
        return tasks.ToList();
    }

    public async Task<Task> GetTaskById(int id)
    {
        return await taskRepository.GetTaskById(id);
    }

    public async Task<Task?> CreateTask(Task task)
    {
        var taskId = await taskRepository.InsertTask(task);
        var taskVectorId = await embeddingProcessor.ProcessTask(task);
        if (!string.IsNullOrEmpty(taskVectorId))
        {
            await taskRepository.LinkToVector(taskId, taskVectorId);
        }
        
        if (taskId == 0)
        {
            return null;
        }

        return await taskRepository.GetTaskById(taskId);
    }

    public async Task<Task> UpdateTask(Task task)
    {
        return await taskRepository.UpdateTask(task);
    }

    public async System.Threading.Tasks.Task DeleteTask(int id)
    {
        await taskRepository.DeleteTask(id);
    }

    public async Task<DataBatch> GetTaskDataBatch(int id)
    {
        var dataBatch = new DataBatch();
        
        try
        {
            dataBatch.Task = await taskRepository.GetTaskById(id);
            dataBatch.Assignments = await assignmentRepository.GetAssignmentsByTaskId(id);
            dataBatch.Comments = await commentRepository.GetCommentsByTaskId(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return dataBatch;
    }
}