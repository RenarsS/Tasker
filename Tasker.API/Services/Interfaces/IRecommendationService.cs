using Microsoft.Extensions.AI;
using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services.Interfaces;

public interface IRecommendationService
{
    Task<Response> GenerateRecommendationResponse(Task task, int relevantTaskCount);
    
}