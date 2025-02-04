﻿using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Domain.Extensions;
using Tasker.Infrastructure.Builders;
using Tasker.Infrastructure.Client.Interfaces;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Processor;

public class EmbeddingProcessor(
    IEmbeddingGenerator<string, Embedding<float>> generator,
    IEmbeddingClient embeddingClient,
    OrderBuilder orderBuilder) : IEmbeddingProcessor
{
    public async Task<GeneratedEmbeddings<Embedding<float>>> Process(object entity)
    {
        var values = new []
        {
            JsonConvert.SerializeObject(entity)
        };

        return await generator.GenerateAsync(values);
    }
    
    public async Task<string> ProcessTask(Task task)
    {
        var taskMeta = task.GetMetadata();
        var taskEmbedding = await Process(task);
        var vectorId = await embeddingClient.UpsertEmbeddings(Collections.Tasks, taskEmbedding, taskMeta);
        return vectorId?.First()!;
    }
    
    public async Task<string> ProcessAssignment(Assignment assignment)
    {
        var assignmentMeta = assignment.GetMetadata();
        var assignmentEmbedding = await Process(assignment);
        var vectorId = await embeddingClient.UpsertEmbeddings(Collections.Assignments, assignmentEmbedding, assignmentMeta);
        return vectorId?.First()!;
    }
    
    public async Task<string> ProcessComment(Comment comment)
    {
        var commentMeta = comment.GetMetadata();
        var commentEmbedding = await Process(comment.Content);
        var vectorId = await embeddingClient.UpsertEmbeddings(Collections.Comments, commentEmbedding, commentMeta);
        return vectorId?.First()!;
    }
    
    public async Task<string> ProcessOrder(Task task, IEnumerable<Assignment>? assignment = null, IEnumerable<Comment>? comments = null)
    {
        orderBuilder.SetTask(task);
        if (assignment != null)
        {
            orderBuilder.SetAssignments(assignment);
        }

        if (comments != null)
        {
            orderBuilder.SetComments(comments);
        }
        
        var order = orderBuilder.Build();
        var orderMeta = order.GetMetadata();
        var orderEmbedding = await Process(order);
        var vectorId = await embeddingClient.UpsertEmbeddings(Collections.Orders, orderEmbedding, orderMeta);
        orderBuilder.Reset();
        return vectorId?.First()!;
    }

    public async Task<string> ProcessQuery(string prompt, Query query)
    {
        var queryMetadata = query.GetMetadata();
        var promptEmbedding = await Process(prompt);
        var vectorId = await embeddingClient.UpsertEmbeddings(Collections.Queries, promptEmbedding, queryMetadata);
        return vectorId?.First()!;
    }
}