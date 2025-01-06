﻿using System.Text;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class RecommendationService(
    IConfiguration configuration,
    IChatClient chatClient, 
    IRetrievalService retrievalService) : IRecommendationService
{
    public async Task<Comment> GenerateRecommendationComment(Task task)
    {
        var options = new ChatOptions();
        var relevantOrders = await retrievalService.GetRelevantOrders(task);
        var prompt = BuildPrompt(task, relevantOrders);
        var response = await chatClient.CompleteAsync(prompt);
        if (!string.IsNullOrEmpty(response.Message.Contents.ToString()))
        {
            return new Comment
            {
                Content = response.Message.Contents.ToString(),
                Task = task.TaskId,
                User = 21
            };
        }

        return new Comment { Content = string.Empty };
    }

    private string BuildPrompt(Task task, IEnumerable<(double, Order)> orders)
    {
        var serializedTask = JsonConvert.SerializeObject(task);
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(configuration["Prompts:TaskResolution"]);
        stringBuilder.AppendLine($"The task for recommendation: {serializedTask}");
        stringBuilder.AppendLine("During retrieval of tasks, some of the relevant to this one is:");
        foreach (var order in orders)
        {
            var serializedOrder = JsonConvert.SerializeObject(order.Item2);
            stringBuilder.AppendLine($"Similarity score: {order.Item1}, task: {serializedOrder}");
        }

        return stringBuilder.ToString();
    }
}