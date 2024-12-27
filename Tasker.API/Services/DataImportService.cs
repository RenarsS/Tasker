using Tasker.API.Services.Interfaces;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Domain.Extensions;
using Tasker.Domain.Import;
using Tasker.Domain.Processor;
using Tasker.Infrastructure.Builders;
using Tasker.Infrastructure.Client;
using Tasker.Infrastructure.Client.Interfaces;
using Tasker.Infrastructure.Repositories;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.API.Services;

public class DataImportService(
    TaskRepository taskRepository, 
    AssignmentRepository assignmentRepository, 
    CommentRepository commentRepository,
    EmbeddingProcessor embeddingProcessor,
    IEmbeddingClient embeddingClient,
    OrderBuilder orderBuilder) : IDataImportService
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
            var task = await ProcessTask(batch.Task);
            taskInserted++;

            foreach (var assignment in batch.Assignments)
            {
                assignment.Task = task;
                await ProcessAssignment(assignment);
                assignmentsInserted++;
            }

            foreach (var comment in batch.Comments)
            {
                comment.Task = task;
                await ProcessComment(comment);
                commentsInserted++;
            }
            
            await ProcessOrder(batch.Task, batch.Assignments, batch.Comments);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        return (taskInserted, assignmentsInserted, commentsInserted);
    }   

    private async Task<int> ProcessTask(Task task)
    {
        var taskId = await taskRepository.InsertTask(task);
        task.TaskId = taskId;
        var taskMeta = task.GetMetadata();
        var taskEmbedding = await embeddingProcessor.Process(task);
        await embeddingClient.UpsertEmbedding(Collections.Tasks, taskEmbedding, taskMeta);
        return taskId;
    }
    
    private async System.Threading.Tasks.Task ProcessAssignment(Assignment assignment)
    {
        await assignmentRepository.InsertAssignment(assignment);
        var assignmentMeta = assignment.GetMetadata();
        var assignmentEmbedding = await embeddingProcessor.Process(assignment);
        await embeddingClient.UpsertEmbedding(Collections.Assignments, assignmentEmbedding, assignmentMeta);
    }
    
    private async System.Threading.Tasks.Task ProcessComment(Comment comment)
    {
        await commentRepository.InsertComment(comment);
        var commentMeta = comment.GetMetadata();
        var commentEmbedding = await embeddingProcessor.Process(comment);
        await embeddingClient.UpsertEmbedding(Collections.Comments, commentEmbedding, commentMeta);
    }

    private async System.Threading.Tasks.Task ProcessOrder(Task task, IEnumerable<Assignment> assignment, IEnumerable<Comment> comments)
    {
        orderBuilder.SetTask(task);
        orderBuilder.SetAssignments(assignment);
        orderBuilder.SetComments(comments);
        var order = orderBuilder.Build();
        var orderMeta = order.GetMetadata();
        var orderEmbedding = await embeddingProcessor.Process(order);
        await embeddingClient.UpsertEmbedding(Collections.Orders, orderEmbedding, orderMeta);
        orderBuilder.Reset();
    }
}