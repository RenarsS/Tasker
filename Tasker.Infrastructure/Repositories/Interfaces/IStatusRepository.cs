using Tasker.Domain.MasterData;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface IStatusRepository
{
    Task<IEnumerable<Status>> GetStatuses();
    
    Task<Status> GetStatusById(int id);
}