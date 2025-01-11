using System.Text;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants;
using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class RecommendationService(
    IConfiguration configuration,
    IChatClient chatClient, 
    IRetrievalService retrievalService,
    IPromptService promptService) : IRecommendationService
{
    public async Task<Comment> GenerateRecommendationComment(Task task)
    {
        var options = new ChatOptions();
        var relevantOrders = await retrievalService.GetRelevantOrders(task);
        var prompt = await BuildPrompt(task, relevantOrders, Prompts.TaskRetrieval);
        var response = await chatClient.CompleteAsync(prompt);
        if (!string.IsNullOrEmpty(response.Message.Text))
        {
            return new Comment
            {
                Content = response.Message.Text,
                Task = task.TaskId,
                User = 22
            };
        }

        return new Comment { Content = string.Empty };
    }

    private async Task<string> BuildPrompt(Task task, IEnumerable<(double, Order)> orders, string promptName)
    {
        var serializedTask = JsonConvert.SerializeObject(task);
        var stringBuilder = new StringBuilder();
        var prompt = await promptService.GetPrompt(promptName);
        stringBuilder.AppendLine(prompt);
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