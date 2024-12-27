using System.Data;
using Dapper;
using Dapper.Oracle;
using Tasker.Domain.Constants.Packages;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Database;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.Infrastructure.Repositories;

public class AssignmentRepository(OracleDbService oracleDbService) : IAssignmentRepository
{
    public async Task<IEnumerable<Assignment>> GetAssignments()
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("c_assignments", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Assignment>($"{Packages.AssignmentsCore}.get_assignments", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Assignment> GetAssignmentById(int id)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_assignment_id", dbType: OracleMappingType.Int32, value: id, direction: ParameterDirection.Input);
        parameters.Add("c_assignment", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Assignment>($"{Packages.AssignmentsCore}.get_assignment_by_id", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Assignment>> GetAssignmentsByTaskId(int taskId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: taskId, direction: ParameterDirection.Input);
        parameters.Add("c_assignments", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Assignment>($"{Packages.AssignmentsCore}.get_assignments_by_task_id", parameters, commandType: CommandType.StoredProcedure);
    }
    
    public async Task<IEnumerable<Assignment>> GetAssignmentsByUserId(int userId)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_user_id", dbType: OracleMappingType.Int32, value: userId, direction:ParameterDirection.Input);
        parameters.Add("c_assignments", dbType: OracleMappingType.RefCursor,direction: ParameterDirection.Output);
        
        return await connection.QueryAsync<Assignment>($"{Packages.AssignmentsCore}.get_assignments_by_user_id", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Assignment> InsertAssignment(Assignment assignment)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: assignment.Task, direction: ParameterDirection.Input);
        parameters.Add("i_user_id", dbType: OracleMappingType.Int32, value: assignment.User, direction: ParameterDirection.Input);
        parameters.Add("i_status_id", dbType: OracleMappingType.Int32, value: assignment.Status, direction: ParameterDirection.Input);
        parameters.Add("c_inserted_assignment", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Assignment>($"{Packages.AssignmentsCore}.insert_assignment", parameters, commandType: CommandType.StoredProcedure);
    }

    public async Task<Assignment> UpdateAssignment(Assignment assignment)
    {
        using var connection = oracleDbService.CreateConnection();
        var parameters = new OracleDynamicParameters();
        parameters.Add("i_assignment_id", dbType: OracleMappingType.Int32, value: assignment.AssignmentId, direction: ParameterDirection.Input);
        parameters.Add("i_task_id", dbType: OracleMappingType.Int32, value: assignment.Task, direction: ParameterDirection.Input);
        parameters.Add("i_user_id", dbType: OracleMappingType.Int32, value: assignment.User, direction: ParameterDirection.Input);
        parameters.Add("i_status_id", dbType: OracleMappingType.Int32, value: assignment.Status, direction: ParameterDirection.Input);
        parameters.Add("c_updated_comment", dbType: OracleMappingType.RefCursor, direction: ParameterDirection.Output);
        
        return await connection.QueryFirstAsync<Assignment>($"{Packages.AssignmentsCore}.update_assignment", parameters, commandType: CommandType.StoredProcedure);
    }
}