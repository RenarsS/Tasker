using Tasker.API.Infrastructure.Clients;

namespace Tasker.Services;

public interface IMasterDataService
{
    Task<IEnumerable<TaskType>> GetTaskTypesAsync();
    
    Task<IEnumerable<Status>> GetStatusAsync();
}