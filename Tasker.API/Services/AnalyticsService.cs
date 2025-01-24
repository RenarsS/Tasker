using System.Numerics.Tensors;
using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Client.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class AnalyticsService(
    ITaskService taskService,
    ICommentService commentService,
    IResponseService responseService,
    IQueryService queryService,
    IEmbeddingClient embeddingClient) : IAnalyticsService
{

    public async System.Threading.Tasks.Task CreateTaskRetrievalRating(Task task, IEnumerable<(double, Order)> retrievedTasks)
    {
        foreach (var retrievedTask in retrievedTasks)
        {
            var adjustedSimilarity = retrievedTask.Item1;
            var taskRetrievalRanking = new TaskRetrievalRating()
            {
                TaskId = task.TaskId,
                RetrievalTaskId = retrievedTask.Item2.TaskId,
                Rating = (float) adjustedSimilarity,
            };

            await taskService.CreateTaskRetrievalRating(taskRetrievalRanking);
        }
    }
    
    public async System.Threading.Tasks.Task CreateQueryResponseRating(string prompt, Response response)
    {
        var query = new Query { ResponseId = response.ResponseId };
        var insertedQuery = await queryService.CreateQuery(prompt, query);
        var queryResponseRating = new QueryResponseRating
        {
            QueryId = insertedQuery.QueryId,
            ResponseId = response.ResponseId
        };
        
        await queryService.CreateQueryResponseRating(queryResponseRating);
    }
    
    public async System.Threading.Tasks.Task CreateResponseRetrievalRating(Response response, IEnumerable<Order> retrievedTasks)
    {
        foreach (var retrievedTask in retrievedTasks)
        {
            var responseRetrievalRanking = new ResponseRetrievalRating
            {
                ResponseId = response.ResponseId,
                TaskId = retrievedTask.TaskId
            };
        
            await responseService.CreateResponseRetrievalRating(responseRetrievalRanking);
        }
    }

    public async System.Threading.Tasks.Task RateQueryResponses()
    {
        var queryResponseRatings = await queryService.GetUnratedQueryResponseRatings();
        var ratings = queryResponseRatings.ToList();
        if (ratings.Count == 0)
        {
            return;
        }
        
        foreach (var queryResponseRating in ratings)
        {
            var query = await queryService.GetQueryById(queryResponseRating.QueryId);
            var response = await responseService.GetResponseById(queryResponseRating.ResponseId);
            var comment = await commentService.GetCommentById(response.CommentId);

            var queryEmbedding = await embeddingClient.GetEmbedding(Collections.Queries, query.VectorId!, true);
            var responseEmbedding = await embeddingClient.GetEmbedding(Collections.Comments, comment.VectorId!, true);
            if (queryEmbedding == null || responseEmbedding == null)
            {
                continue;
            }
            
            var rating = CalculateVectorSimilarity(queryEmbedding.Embedding, responseEmbedding.Embedding);
            await queryService.UpdateQueryResponseRating(queryResponseRating.RatingId, rating);
        }
    }

    public async System.Threading.Tasks.Task RateResponseRetrievals()
    {
        var responseRetrievalRatings = await responseService.GetUnratedResponseRetrievalRatings();
        var ratings = responseRetrievalRatings.ToList();
        if (ratings.Count == 0)
        {
            return;
        }

        foreach (var responseRetrievalRating in ratings)
        {
            var response = await responseService.GetResponseById(responseRetrievalRating.ResponseId);
            var comment = await commentService.GetCommentById(response.CommentId);
            var task = await taskService.GetTaskById(responseRetrievalRating.TaskId);
            
            var responseEmbedding = await embeddingClient.GetEmbedding(Collections.Comments, comment.VectorId!, true);
            var taskEmbedding = await embeddingClient.GetEmbedding(Collections.Orders, task.OrderVectorId!, true);
            if (responseEmbedding == null && taskEmbedding == null)
            {
                continue;
            }
            
            var rating = CalculateVectorSimilarity(responseEmbedding.Embedding, taskEmbedding.Embedding);
            await responseService.UpdateResponseRetrievalRating(responseRetrievalRating.RatingId, rating);
        }
    }


    private float CalculateVectorSimilarity(ReadOnlyMemory<float> vector1, ReadOnlyMemory<float> vector2)
        => TensorPrimitives.CosineSimilarity(vector1.Span, vector2.Span);

}