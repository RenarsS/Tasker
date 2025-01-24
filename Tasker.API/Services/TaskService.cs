using System.ComponentModel.Design;
using MassTransit;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Domain.Events;
using Tasker.Domain.Import;
using Tasker.Domain.Settings;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class TaskService(
    IConfiguration configuration,
    ITaskRepository taskRepository, 
    IAssignmentService assignmentService, 
    ICommentService  commentService,
    IEmbeddingProcessor embeddingProcessor,
    IPublishEndpoint publishEndpoint) : ITaskService
{
    private readonly EmbeddingSettings _embeddingSettings = configuration.GetSection("EmbeddingSettings").Get<EmbeddingSettings>()!;
    private readonly RecommendationSettings _recommendationSettings = configuration.GetSection("RecommendationSettings").Get<RecommendationSettings>()!;
    
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
        
        string taskVectorId = String.Empty;
        string taskOrderVectorId = String.Empty;
        if (_embeddingSettings.IsEnabled)
        {
            taskVectorId = await embeddingProcessor.ProcessTask(task);
            taskOrderVectorId = await embeddingProcessor.ProcessOrder(task);
        }
        
        task.TaskId = taskId;
        task.VectorId = taskVectorId;
        task.OrderVectorId = taskOrderVectorId;

        if (!string.IsNullOrEmpty(taskVectorId))
        {
            await taskRepository.LinkToVector(taskId, taskVectorId);
        }

        if (!string.IsNullOrEmpty(taskOrderVectorId))
        {
            await taskRepository.LinkToOrderVector(taskId, taskOrderVectorId);
        }
        
        if (_recommendationSettings.IsEnabled)
        {
            var newTaskCreated = new NewTaskCreated
            {
                RelevantTaskCount = [1, 4, 7, 10, 12],
                Task = task
            };
            await publishEndpoint.Publish(newTaskCreated);
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
            dataBatch.Task = await GetTaskById(id);
            dataBatch.Assignments = await assignmentService.GetAssignmentsByTaskId(id);
            dataBatch.Comments = await commentService.GetCommentsByTaskId(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return dataBatch;
    }

    public async Task<Task> GetTaskByVectorId(string vectorId)
    {
        return await taskRepository.GetTaskByVectorId(vectorId);
    }

    public async Task<bool> EmbedTasks()
    {
        var tasks = await taskRepository.GetTasksNotEmbedded();
        var orders =  await taskRepository.GetOrdersNotEmbedded();
        try
        {
            foreach (var task in tasks)
            {
                var vectorId = await embeddingProcessor.ProcessTask(task);
                await taskRepository.LinkToVector(task.TaskId, vectorId);
            }

            foreach (var order in orders)
            {
                var vectorId = await embeddingProcessor.ProcessOrder(order);
                await taskRepository.LinkToOrderVector(order.TaskId, vectorId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }

    public async System.Threading.Tasks.Task CreateTaskRetrievalRating(TaskRetrievalRating taskRetrievalRating)
    {
        await taskRepository.InsertTaskRetrievalRating(taskRetrievalRating);
    }
}