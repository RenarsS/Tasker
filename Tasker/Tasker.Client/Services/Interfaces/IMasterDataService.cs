using Tasker.Domain.Enums;
using Tasker.Domain.MasterData;

namespace Tasker.Client.Services.Interfaces;

public interface IMasterDataService
{
    Task<IEnumerable<MasterData>> GetMasterData(MasterDataType masterDataType);
}