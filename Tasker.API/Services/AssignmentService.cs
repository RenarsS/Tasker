using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.API.Services;

public class AssignmentService(IAssignmentRepository assignmentRepository, IEmbeddingProcessor embeddingProcessor) : IAssignmentService
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
        var insertedAssignment = await assignmentRepository.InsertAssignment(assignment);
        // var assignmentVectorId = await embeddingProcessor.ProcessAssignment(insertedAssignment);
        // if (!string.IsNullOrEmpty(assignmentVectorId))
        // {
        //     await assignmentRepository.LinkToVector(insertedAssignment.AssignmentId, assignmentVectorId);
        // }

        return insertedAssignment;
    }

    public async Task<Assignment> UpdateAssignment(Assignment assignment)
    {
        return await assignmentRepository.UpdateAssignment(assignment);
    }
}