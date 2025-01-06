using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services.Interfaces;

public interface IRecommendationService
{
    Task<Comment> GenerateRecommendationComment(Task task);
}