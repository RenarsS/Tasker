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
    IResponseService responseService,
    IPromptService promptService,
    IAnalyticsService analyticsService) : IRecommendationService
{
    public async Task<Response> GenerateRecommendationResponse(Task task, int relevantTaskCount = 5)
    {
        var options = new ChatOptions();
        var relevantOrders = await retrievalService.GetRelevantOrders(task, relevantTaskCount);
        var prompt = await BuildPrompt(task, relevantOrders, Prompts.TaskRetrieval);
        var chatCompletion = await chatClient.CompleteAsync(prompt);
        
        var response = new Response
        {
            Content = chatCompletion.Message.Text ?? string.Empty,
            InputTokenCount = (int) chatCompletion.Usage!.InputTokenCount!,
            OutputTokenCount = (int) chatCompletion.Usage!.OutputTokenCount!,
            TotalTokenCount = (int) chatCompletion.Usage!.TotalTokenCount!,
            UserId = 22,
        };
        
        var insertedResponse = await responseService.CreateResponse(response);
        response.ResponseId = insertedResponse.ResponseId;
        var relevantOrderTasks = relevantOrders.Select(o => o.Item2);

        await analyticsService.CreateQueryResponseRating(prompt, response);
        await analyticsService.CreateResponseRetrievalRating(response, relevantOrderTasks);
        await analyticsService.CreateTaskRetrievalRating(task, relevantOrders);
        
         return response;
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