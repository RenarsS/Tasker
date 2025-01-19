using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services.Interfaces;

public interface IAnalyticsService
{
    System.Threading.Tasks.Task CreateTaskRetrievalRating(Task task, IEnumerable<(double, Order)> retrievedTasks);

    System.Threading.Tasks.Task CreateQueryResponseRating(string prompt, Response response);

    System.Threading.Tasks.Task CreateResponseRetrievalRating(Response response, IEnumerable<Order> retrievedTasks);
}