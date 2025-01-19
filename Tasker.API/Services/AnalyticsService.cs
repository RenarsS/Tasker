using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class AnalyticsService(
    ITaskService taskService,
    IResponseService responseService,
    IQueryService queryService) : IAnalyticsService
{

    public async System.Threading.Tasks.Task CreateTaskRetrievalRating(Task task, IEnumerable<(double, Order)> retrievedTasks)
    {
        foreach (var retrievedTask in retrievedTasks)
        {
            var taskRetrievalRanking = new TaskRetrievalRating()
            {
                TaskId = task.TaskId,
                RetrievalTaskId = retrievedTask.Item2.TaskId,
                Rating = (float) retrievedTask.Item1
            };

            await taskService.CreateTaskRetrievalRating(taskRetrievalRanking);
        }
    }
    
    public async System.Threading.Tasks.Task CreateQueryResponseRating(string prompt, Response response)
    {
        var query = new Query { ResponseId = response.ResponseId};
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
}