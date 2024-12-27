using Tasker.Domain.MasterData;

namespace Tasker.API.Services.Interfaces;

public interface ITaskTypeService
{
    public Task<IEnumerable<TaskType>> GetAllTaskTypes();
    
    public Task<TaskType> GetTaskTypeById(int id);
}