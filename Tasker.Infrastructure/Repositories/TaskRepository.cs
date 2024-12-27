using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Repositories;


public class TaskRepository(OracleDbService oracleDbService) : ITaskRepository
{
    
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
}