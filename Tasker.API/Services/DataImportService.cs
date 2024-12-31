using Tasker.API.Services.Interfaces;
using Tasker.Domain.Import;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.API.Services;

public class DataImportService(
    ITaskRepository taskRepository, 
    IAssignmentRepository assignmentRepository, 
    ICommentRepository commentRepository,
    IEmbeddingProcessor embeddingProcessor) : IDataImportService
{
    public async Task<(int, int, int)> ImportDataBatches(IEnumerable<DataBatch> batches)
    {
        (int tasks, int assignments, int comments) statistics = (0, 0, 0);

        foreach (var batch in batches)
        {
            (int tasks, int assignments, int comments) stat = await ImportDataBatch(batch);
            statistics.tasks += stat.tasks;
            statistics.assignments += stat.assignments;
            statistics.comments += stat.comments;
        }

        return statistics;
    }

    public async Task<(int, int, int)> ImportDataBatch(DataBatch batch)
    {
        int taskInserted = 0;
        int assignmentsInserted = 0;
        int commentsInserted = 0;

        try
        {
            var taskId = await taskRepository.InsertTask(batch.Task);
            batch.Task.TaskId = taskId;
            var taskVectorId = await embeddingProcessor.ProcessTask(batch.Task);
            if (!string.IsNullOrEmpty(taskVectorId))
            {
                await taskRepository.LinkToVector(batch.Task.TaskId, taskVectorId);
            }
            
            taskInserted++;

            foreach (var assignment in batch.Assignments)
            {
                assignment.Task = taskId;
                var insertedAssignment = await assignmentRepository.InsertAssignment(assignment);
                var assignmentVectorId = await embeddingProcessor.ProcessAssignment(insertedAssignment);
                if (!string.IsNullOrEmpty(assignmentVectorId))
                {
                    await assignmentRepository.LinkToVector(insertedAssignment.AssignmentId, assignmentVectorId);
                }
                
                assignmentsInserted++;
            }

            foreach (var comment in batch.Comments)
            {
                comment.Task = taskId;
                var insertedComment = await commentRepository.InsertComment(comment);
                var commentVectorId = await embeddingProcessor.ProcessComment(insertedComment);
                if (!string.IsNullOrEmpty(commentVectorId))
                {
                    await assignmentRepository.LinkToVector(insertedComment.CommentId, commentVectorId);
                }
                
                commentsInserted++;
            }
            
            await embeddingProcessor.ProcessOrder(batch.Task, batch.Assignments, batch.Comments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return (taskInserted, assignmentsInserted, commentsInserted);
    }   
}