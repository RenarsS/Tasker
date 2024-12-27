using Tasker.API.Infrastructure.Clients;

namespace Tasker.Services;

public class MasterDataService(TaskerApiClient taskerApiClient) : IMasterDataService
{
    public async Task<IEnumerable<TaskType>> GetTaskTypesAsync() 
        => await taskerApiClient.GetApiTaskTypesAllAsync();

    public async Task<IEnumerable<Status>> GetStatusAsync()
        => await taskerApiClient.GetApiStatusesAllAsync();
}