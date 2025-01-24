using Tasker.Domain.DTO;
using Task = System.Threading.Tasks.Task;

namespace Tasker.API.Services.Interfaces;

public interface ICommentService
{
    public Task<IEnumerable<Comment>> GetAllComments();
    
    public Task<Comment> GetCommentById(int id);
    
    public Task<IEnumerable<Comment>> GetCommentsByTaskId(int taskId);
    
    public Task<Comment> CreateComment(Comment comment);
    
    public Task<Comment> UpdateComment(Comment comment);
    
    public Task DeleteTask(int id);

    Task<bool> EmbedComments();
}