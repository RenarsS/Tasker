using Tasker.API.Services.Interfaces;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Repositories;
using Tasker.Infrastructure.Repositories.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services;

public class CommentService(CommentRepository commentRepository) : ICommentService
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

    public async Task<Comment?> CreateComment(Comment comment)
    {
        return await commentRepository.InsertComment(comment);
    }

    public async Task<Comment> UpdateComment(Comment comment)
    {
        return await commentRepository.UpdateComment(comment);
    }

    public async Task DeleteTask(int id)
    {
        await commentRepository.DeleteComment(id);
    }
}