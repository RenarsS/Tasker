using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.Infrastructure.Repositories;

public class UserRepository(OracleDbService oracleDbService) : IUserRepository
{
    public async Task<IEnumerable<User>> GetUsers()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_users", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<User>($"{Packages.UsersCore}.get_users", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<User> GetUserById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_user_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("c_user", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<User>($"{Packages.UsersCore}.get_user_by_id", parameters, commandType: CommandType.StoredProcedure);

    }
}