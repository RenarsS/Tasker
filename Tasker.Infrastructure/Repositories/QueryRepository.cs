using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories;

public class QueryRepository(OracleDbService oracleDbService) : IQueryRepository
{
    public async Task<Query> InsertQuery(Query query)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_response_id", dbType: OracleMappingType.Int32, value: query.ResponseId, direction: ParameterDirection.Input);
        parameters.Add("i_vector_id", dbType: OracleMappingType.NVarchar2, value: query.VectorId, direction: ParameterDirection.Input);
        parameters.Add("c_inserted_query", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Query>($"{Packages.QueriesCore}.insert_query", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task InsertQueryResponseRating(QueryResponseRating queryResponseRating)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_query_id", dbType: OracleMappingType.Int32, value: queryResponseRating.QueryId, direction: ParameterDirection.Input);
        parameters.Add("i_response_id", dbType: OracleMappingType.Int32, value: queryResponseRating.ResponseId, direction: ParameterDirection.Input);
        parameters.Add("i_rating", dbType: OracleMappingType.Double, value: queryResponseRating.Rating, direction: ParameterDirection.Input);
        parameters.Add("o_query_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        
        await connection.ExecuteAsync($"{Packages.QueriesCore}.insert_query_response_rating", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task LinkToVector(int id, string vectorId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_query_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("i_vector_id", dbType: OracleMappingType.NVarchar2, value: vectorId, direction: ParameterDirection.Input);
        
        await connection.ExecuteAsync($"{Packages.QueriesCore}.link_query_to_vector", parameters, commandType: CommandType.StoredProcedure);
    }
}