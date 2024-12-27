using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Domain.MasterData;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.Infrastructure.Repositories;

public class StatusRepository(OracleDbService oracleDbService) : IStatusRepository
{
    public async Task<IEnumerable<Status>> GetStatuses()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_statuses", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Status>($"{Packages.StatusesCore}.get_statuses", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Status> GetStatusById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_status_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("c_status", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Status>($"{Packages.StatusesCore}.get_status_by_id", parameters, commandType: CommandType.StoredProcedure);

    }
}