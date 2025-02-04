﻿using Microsoft.SemanticKernel.Memory;
using Newtonsoft.Json;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Builders;
using Tasker.Infrastructure.Client.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class RetrievalService(
    IConfiguration configuration,
    IEmbeddingClient embeddingClient, 
    ITaskService taskService, 
    IAssignmentService assignmentService, 
    ICommentService commentService, 
    OrderBuilder orderBuilder) : IRetrievalService
{
    public async Task<List<(double, Order)>> GetRelevantOrders(Task task,  int relevantTaskCount = 5)
    {
        var relevanceScore = configuration.GetValue<double>("RetrievalSettings:RelevanceScore");
        var orders = new List<(double, Order)>();
        var taskMemoryRecord = await embeddingClient.GetEmbedding(Collections.Tasks, task.VectorId, true);
        var relevantTaskEmbeddings = embeddingClient.GetNearestMatches(Collections.Tasks, taskMemoryRecord.Embedding, relevantTaskCount, relevanceScore).ToBlockingEnumerable();
        foreach (var (memoryRecord, similarity) in relevantTaskEmbeddings)
        {
            var order = await RetrieveOrderByVectorId(memoryRecord);
            var orderPair = (similarity, order);
            orders.Add((orderPair.Item1, orderPair.Item2));
        }
        
        return orders;
    }

    private async Task<Order> RetrieveOrderByVectorId(MemoryRecord memoryRecord)
    {
        try
        {
            var task = await taskService.GetTaskByVectorId(memoryRecord.Key);
            var assignments = await assignmentService.GetAssignmentsByTaskId(task.TaskId);
            var comments = await commentService.GetCommentsByTaskId(task.TaskId);
            orderBuilder.SetTask(task);
            orderBuilder.SetAssignments(assignments);
            orderBuilder.SetComments(comments);
            return orderBuilder.Build();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}