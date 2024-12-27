using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Repositories;

namespace Tasker.API.Services;

public class AssignmentService(AssignmentRepository assignmentRepository) : IAssignmentService
{
    public async Task<IEnumerable<Assignment>> GetAssignments()
    {
        return await assignmentRepository.GetAssignments();
    }

    public async Task<Assignment> GetAssignmentById(int id)
    {
        return await assignmentRepository.GetAssignmentById(id);
    }

    public async Task<IEnumerable<Assignment>> GetAssignmentsByTaskId(int taskId)
    {
        return await assignmentRepository.GetAssignmentsByTaskId(taskId);
    }

    public async Task<IEnumerable<Assignment>> GetAssignmentsByUserId(int userId)
    {
        return await assignmentRepository.GetAssignmentsByUserId(userId);
    }

    public async Task<Assignment> CreateAssignment(Assignment assignment)
    {
        return await assignmentRepository.InsertAssignment(assignment);
    }

    public async Task<Assignment> UpdateAssignment(Assignment assignment)
    {
        return await assignmentRepository.UpdateAssignment(assignment);
    }
}