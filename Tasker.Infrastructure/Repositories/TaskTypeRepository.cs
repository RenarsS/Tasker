using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Domain.MasterData;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.Infrastructure.Repositories;

public class TaskTypeRepository(OracleDbService oracleDbService) : ITaskTypeRepository
{
    public async Task<IEnumerable<TaskType>> GetTaskTypes()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_task_types", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<TaskType>($"{Packages.TaskTypesCore}.get_task_types", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<TaskType> GetTaskTypeById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_type_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("c_task_type", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<TaskType>($"{Packages.TaskTypesCore}.get_task_types_by_id", parameters, commandType: CommandType.StoredProcedure);

    }
}