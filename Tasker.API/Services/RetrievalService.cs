using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Builders;
using Tasker.Infrastructure.Client.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class RetrievalService(
    IEmbeddingClient embeddingClient, 
    ITaskService taskService, 
    IAssignmentService assignmentService, 
    ICommentService commentService, 
    OrderBuilder orderBuilder) : IRetrievalService
{
    public async Task<List<(double, Order)>> GetRelevantOrders(Task task)
    {
        var orders = new List<(double, Order)>();
        var taskMemoryRecord = await embeddingClient.GetEmbedding(Collections.Tasks, task.VectorId, true);
        var relevantTaskEmbeddings = embeddingClient.GetNearestMatches(Collections.Tasks, taskMemoryRecord.Embedding, 10, 0.7).ToBlockingEnumerable();
        foreach (var (memoryRecord, similarity) in relevantTaskEmbeddings)
        {
            var order = await RetrieveOrderByVectorId(memoryRecord.Key);
            var orderPair = (similarity, order);
            orders.Add((orderPair.Item1, orderPair.Item2));
        }
        
        return orders;
    }

    private async Task<Order> RetrieveOrderByVectorId(string vectorId)
    {
        var task =  await taskService.GetTaskByVectorId(vectorId);
        var assignments = await assignmentService.GetAssignmentsByTaskId(task.TaskId);
        var comments = await commentService.GetCommentsByTaskId(task.TaskId);
        orderBuilder.SetTask(task);
        orderBuilder.SetAssignments(assignments);
        orderBuilder.SetComments(comments);
        return orderBuilder.Build();
    }
}