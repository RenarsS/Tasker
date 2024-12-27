using Tasker.Domain.DTO;
using Tasker.Domain.MasterData;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface ITaskTypeRepository
{
    Task<IEnumerable<TaskType>> GetTaskTypes();
    
    Task<TaskType> GetTaskTypeById(int id);
}