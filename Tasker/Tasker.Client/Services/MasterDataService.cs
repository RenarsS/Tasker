using Microsoft.AspNetCore.Components;
using Tasker.API;
using Tasker.Client.Services.Interfaces;
using Tasker.Domain.Enums;
using Tasker.Domain.MasterData;
using Status = Tasker.API.Status;
using TaskType = Tasker.API.TaskType;

namespace Tasker.Client.Services;

public class MasterDataService(ITaskerClient taskerClient) : IMasterDataService
{
    public async Task<IEnumerable<MasterData>> GetMasterData(MasterDataType masterDataType)
    {
        List<MasterData> masterDataList;
        switch (masterDataType)
        {
            case MasterDataType.Status:
                var statusList = await taskerClient.GetApiStatusesAllAsync();
                masterDataList = statusList.Select(s => new MasterData
                {
                    Id = s.StatusId,
                    Name = s.Name,
                    Description = s.Description,
                }).ToList();
                break;
            
            case MasterDataType.TaskType:
                var taskTypeList = await taskerClient.GetApiTaskTypesAllAsync();
                masterDataList = taskTypeList.Select(s => new MasterData
                {
                    Id = s.TaskTypeId,
                    Name = s.Name,
                    Description = s.Description,
                }).ToList();
                break;
            
            default:
                return new List<MasterData>();
        }
        
        return masterDataList;
    }
}