using Tasker.Domain.DTO;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Repositories.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetComments();
    
    Task<Comment> GetCommentById(int id);
    
    Task<IEnumerable<Comment>> GetCommentsByTaskId(int taskId);
    
    Task<Comment> InsertComment(Comment comment);
    
    Task<Comment> UpdateComment(Comment comment);
    
    Task DeleteComment(int id);
}