using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Processor.Interfaces;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services;

public class CommentService(ICommentRepository commentRepository, IEmbeddingProcessor embeddingProcessor) : ICommentService
{
    public async Task<IEnumerable<Comment>> GetAllComments()
    {
        return await commentRepository.GetComments();
    }

    public async Task<Comment> GetCommentById(int id)
    {
        return await commentRepository.GetCommentById(id);
    }

    public async Task<IEnumerable<Comment>> GetCommentsByTaskId(int taskId)
    {
        return await commentRepository.GetCommentsByTaskId(taskId);
    }

    public async Task<Comment> CreateComment(Comment comment)
    {
        var insertedComment = await commentRepository.InsertComment(comment);
        var commentVectorId = await embeddingProcessor.ProcessComment(insertedComment);
        if (!string.IsNullOrEmpty(commentVectorId))
        {
             await commentRepository.LinkToVector(insertedComment.CommentId, commentVectorId);
        }
        
        return insertedComment;
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        return await commentRepository.UpdateComment(comment);
    }

    public async Task DeleteTask(int id)
    {
        await commentRepository.DeleteComment(id);
    }

    public async Task<bool> EmbedComments()
    {
        var comments = await commentRepository.GetCommentsNotEmbedded();
        try
        {
            foreach (var comment in comments)
            {
                var vectorId = await embeddingProcessor.ProcessComment(comment);
                await commentRepository.LinkToVector(comment.CommentId, vectorId);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }

        return true;
    }
}