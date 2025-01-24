using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO.Analytics;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Repositories;


public class TaskRepository(OracleDbService oracleDbService) : ITaskRepository
{
    private const string sql = @"
        SELECT
            t.task_id AS TaskId,
            t.task_type AS TaskType,
            t.title AS Title,
            t.description AS Description,
            t.status AS Status,
            t.created_by AS CreatedBy,
            t.created_at AS CreatedAt,
            t.updated_at AS UpdatedAt,
            t.vector_id AS VectorId,
            t.due AS Due
        FROM
            tasker.tasks t
        WHERE 
            1 = 1
    ";
    public async Task<IEnumerable<Task>> GetTasks()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_tasks", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Task>($"{Packages.TasksCore}.get_tasks", parameters, commandType: CommandType.StoredProcedure);
    }
    
    public async Task<Task> GetTaskById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: id);
        parameters.Add("c_task", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Task>($"{Packages.TasksCore}.get_task_by_id", parameters, commandType: CommandType.StoredProcedure);
    }
    
    public async Task<int> InsertTask(Task task)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_type", dbType: OracleMappingType.Int32, value: task.TaskType, direction: ParameterDirection.Input);
        parameters.Add("i_title", dbType: OracleMappingType.NVarchar2, value: task.Title, direction: ParameterDirection.Input);
        parameters.Add("i_description", dbType: OracleMappingType.NVarchar2, value: task.Description, direction: ParameterDirection.Input);
        parameters.Add("i_status", dbType: OracleMappingType.Int32, value: task.Status, direction: ParameterDirection.Input);
        parameters.Add("i_created_by", dbType:OracleMappingType.Int32, value: task.CreatedBy, direction: ParameterDirection.Input);
        parameters.Add("i_due", dbType: OracleMappingType.Date, value: task.Due, direction: ParameterDirection.Input);
        parameters.Add("o_new_task_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        
        await connection.ExecuteAsync($"{Packages.TasksCore}.insert_task", parameters, commandType: CommandType.StoredProcedure);
        
        return parameters.Get<int>("o_new_task_id");
    }
    
    public async Task<Task> UpdateTask(Task task)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: task.TaskId, direction: ParameterDirection.Input); 
        parameters.Add("i_task_type", dbType: OracleMappingType.Int32, value: task.TaskType, direction: ParameterDirection.Input);
        parameters.Add("i_title", dbType: OracleMappingType.NVarchar2, value: task.Title, direction: ParameterDirection.Input);
        parameters.Add("i_description", dbType: OracleMappingType.NVarchar2, value: task.Description, direction: ParameterDirection.Input);
        parameters.Add("i_status", dbType: OracleMappingType.Int32, value: task.Status, direction: ParameterDirection.Input);
        parameters.Add("i_due", dbType: OracleMappingType.Date, value: task.Due, direction: ParameterDirection.Input);
        parameters.Add("c_updated_task", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);

        return await connection.QueryFirstAsync<Task>($"{Packages.TasksCore}.update_task", parameters, commandType: CommandType.StoredProcedure);
    }

    public async System.Threading.Tasks.Task DeleteTask(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input); 
        
        await connection.ExecuteAsync($"{Packages.TasksCore}.delete_task", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Task> GetTaskByVectorId(string vectorId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_vector_id", dbType: OracleMappingType.NVarchar2, value: vectorId);
        parameters.Add("c_task", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Task>($"{Packages.TasksCore}.get_task_by_vector_id", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Task>> GetTasksNotEmbedded()
    {
        var connection = oracleDbService.CreateConnection();
        var query = sql + "AND t.vector_id IS NULL";
        return await connection.QueryAsync<Task>(query);
    }
    public async Task<IEnumerable<Task>> GetOrdersNotEmbedded()
    {
        var connection = oracleDbService.CreateConnection();
        var query = sql + "AND t.order_vector_id IS NULL";
        return await connection.QueryAsync<Task>(query);
    }

    public async System.Threading.Tasks.Task InsertTaskRetrievalRating(TaskRetrievalRating taskRetrievalRating)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: taskRetrievalRating.TaskId, direction: ParameterDirection.Input); 
        parameters.Add("i_retrieved_task_id", dbType: OracleMappingType.Int32, value: taskRetrievalRating.RetrievalTaskId, direction: ParameterDirection.Input);
        parameters.Add("i_rating", dbType: OracleMappingType.Double, value: taskRetrievalRating.Rating, direction: ParameterDirection.Input);
        parameters.Add("o_task_id", dbType: OracleMappingType.Int32, direction: ParameterDirection.Output);
        
        await connection.ExecuteAsync($"{Packages.TasksCore}.insert_task_retrieval_rating", parameters, commandType: CommandType.StoredProcedure);
    }

    public async System.Threading.Tasks.Task LinkToOrderVector(int taskId, string vectorId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: taskId, direction: ParameterDirection.Input); 
        parameters.Add("i_order_vector_id", dbType: OracleMappingType.NVarchar2, value: vectorId, direction: ParameterDirection.Input); 
        
        await connection.ExecuteAsync($"{Packages.TasksCore}.link_to_order_vector", parameters, commandType: CommandType.StoredProcedure);
    }

    public async System.Threading.Tasks.Task LinkToVector(int id, string vectorId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input); 
        parameters.Add("i_vector_id", dbType: OracleMappingType.NVarchar2, value: vectorId, direction: ParameterDirection.Input); 
        
        await connection.ExecuteAsync($"{Packages.TasksCore}.link_to_vector", parameters, commandType: CommandType.StoredProcedure);
    }
    
}