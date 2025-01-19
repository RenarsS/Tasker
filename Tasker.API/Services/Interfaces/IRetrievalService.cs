using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services.Interfaces;

public interface IRetrievalService
{
    Task<List<(double, Order)>> GetRelevantOrders(Task task, int relevantTaskCount);
}