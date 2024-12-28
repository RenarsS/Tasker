using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories;

public class CommentRepository(OracleDbService oracleDbService) : ICommentRepository, IRepository
{
    public async Task<IEnumerable<Comment>> GetComments()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_comments", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Comment>($"{Packages.CommentsCore}.get_comments", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Comment> GetCommentById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("c_comment", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Comment>($"{Packages.CommentsCore}.get_comment_by_id", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByTaskId(int taskId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: taskId, direction: ParameterDirection.Input);
        parameters.Add("c_comments", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Comment>($"{Packages.CommentsCore}.get_comments_by_task_id", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Comment> InsertComment(Comment comment)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: comment.Task, direction: ParameterDirection.Input);
        parameters.Add("i_user_id", dbType: OracleMappingType.NVarchar2, value: comment.User, direction: ParameterDirection.Input);
        parameters.Add("i_content", dbType: OracleMappingType.NVarchar2, value: comment.Content, direction: ParameterDirection.Input);
        parameters.Add("c_inserted_comment", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Comment>($"{Packages.CommentsCore}.insert_comment", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: comment.Task, direction: ParameterDirection.Input);
        parameters.Add("i_content", dbType: OracleMappingType.NVarchar2, value: comment.Content, direction: ParameterDirection.Input);
        parameters.Add("c_updated_comment", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Comment>($"{Packages.CommentsCore}.update_comment", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task DeleteComment(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input); 
        
        await connection.ExecuteAsync($"{Packages.CommentsCore}.delete_comment", parameters, commandType: CommandType.StoredProcedure);
    }
    
    public async System.Threading.Tasks.Task LinkToVector(int id, string vectorId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_comment_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input); 
        parameters.Add("i_vector_id", dbType: OracleMappingType.NVarchar2, value: vectorId, direction: ParameterDirection.Input); 
        
        await connection.ExecuteAsync($"{Packages.CommentsCore}.link_to_vector", parameters, commandType: CommandType.StoredProcedure);
    }
}