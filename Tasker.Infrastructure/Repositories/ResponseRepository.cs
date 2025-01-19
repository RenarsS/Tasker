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

public class ResponseRepository(OracleDbService oracleDbService): IResponseRepository
{
    public async Task<Response> InsertResponse(Response response)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: response.CommentId, direction: ParameterDirection.Input);
        parameters.Add("i_input_token_count", dbType: OracleMappingType.Int32, value: response.InputTokenCount, direction: ParameterDirection.Input);
        parameters.Add("i_output_token_count", dbType: OracleMappingType.Int32, value: response.OutputTokenCount, direction: ParameterDirection.Input);
        parameters.Add("i_total_token_count", dbType: OracleMappingType.Int32, value: response.TotalTokenCount, direction: ParameterDirection.Input);
        parameters.Add("c_inserted_response", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Response>($"{Packages.ResponsesCore}.insert_response", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task InsertQueryResponseRating(ResponseRetrievalRating responseRetrievalRating)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_response_id", dbType: OracleMappingType.Int32, value: responseRetrievalRating.ResponseId, direction: ParameterDirection.Input);
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: responseRetrievalRating.TaskId, direction: ParameterDirection.Input);
        parameters.Add("i_rating", dbType: OracleMappingType.Double, value: responseRetrievalRating.Rating, direction: ParameterDirection.Input);
        parameters.Add("o_response_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        
        await connection.ExecuteAsync($"{Packages.ResponsesCore}.insert_response_retrieval_rating", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task LinkResponseToComment(int responseId, int commentId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_response_id", dbType: OracleMappingType.Int32, value: responseId, direction: ParameterDirection.Input);
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: commentId, direction: ParameterDirection.Input);
        
        await connection.ExecuteAsync($"{Packages.ResponsesCore}.link_response_to_comment", parameters, commandType: CommandType.StoredProcedure);
    }
}