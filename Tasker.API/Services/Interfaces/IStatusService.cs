using Tasker.Domain.MasterData;

namespace Tasker.API.Services.Interfaces;

public interface IStatusService
{
    public Task<IEnumerable<Status>> GetAllStatuses();
    
    public Task<Status> GetStatusById(int id);
}