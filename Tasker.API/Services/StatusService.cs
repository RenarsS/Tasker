using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Domain.MasterData;
using Tasker.Infrastructure.Repositories;

namespace Tasker.API.Services;

public class StatusService(StatusRepository statusRepository) : IStatusService
{
    public async Task<IEnumerable<Status>> GetAllStatuses()
    {
        return await statusRepository.GetStatuses();
    }

    public async Task<Status> GetStatusById(int id)
    {
        return await statusRepository.GetStatusById(id);
    }
}