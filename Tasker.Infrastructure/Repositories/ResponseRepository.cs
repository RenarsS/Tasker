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

    private const string responseRetrievalRatingSql = @"
        SELECT
            r.rating_id AS RatingId,
            r.response_id AS ResponseId,
            r.task_id AS TaskId,
            r.rating AS Rating,
            r.is_processed AS IsProcessed
        FROM
            tasker.response_retrieval_ratings r
        WHERE
            1=1
    ";

    public async Task<Response> GetResponseById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_response_id", dbType: OracleMappingType.Int32, value: id);
        parameters.Add("c_response", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Response>($"{Packages.ResponsesCore}.get_response_by_id", parameters, commandType: CommandType.StoredProcedure);
    }

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

    public async Task InsertResponseRetrievalRating(ResponseRetrievalRating responseRetrievalRating)
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

    public async Task UpdateResponseRetrievalRating(int ratingId, float rating)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_rating_id", dbType: OracleMappingType.Int32, value: ratingId, direction: ParameterDirection.Input);
        parameters.Add("i_rating", dbType: OracleMappingType.Double, value: rating, direction: ParameterDirection.Input);
        
        await connection.ExecuteAsync($"{Packages.ResponsesCore}.update_rating", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<ResponseRetrievalRating>> GetAllUnratedResponseRetrievalRatings()
    {
        const string query = responseRetrievalRatingSql + " AND r.is_processed = 'N'";
        using var connection = oracleDbService.CreateConnection();
        return await connection.QueryAsync<ResponseRetrievalRating>(query);
    }
}