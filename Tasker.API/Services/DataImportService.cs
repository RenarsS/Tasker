using Tasker.API.Services.Interfaces;
using Tasker.Domain.Import;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories.Interfaces;

namespace Tasker.API.Services;

public class DataImportService(
    ITaskService taskService, 
    IAssignmentService assignmentService, 
    ICommentService commentService,
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
            var task = await taskService.CreateTask(batch.Task);
            if (task == null)
            {
                return (taskInserted, assignmentsInserted, commentsInserted);
            }
            
            taskInserted++;

            foreach (var assignment in batch.Assignments)
            {
                assignment.Task = task.TaskId;
                var insertedAssignment = await assignmentService.CreateAssignment(assignment);
                assignment.AssignmentId = insertedAssignment.AssignmentId;
                assignmentsInserted++;
            }

            foreach (var comment in batch.Comments)
            {
                comment.Task = task.TaskId;
                var insertedComment = await commentService.CreateComment(comment);
                comment.CommentId = insertedComment.CommentId;
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