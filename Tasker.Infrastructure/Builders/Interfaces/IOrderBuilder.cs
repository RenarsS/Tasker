using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Builders.Interfaces;

public interface IOrderBuilder
{
    Order Build();

    void Reset();

    void SetTask(Task task);

    void SetAssignments(IEnumerable<Assignment> assignments);
    
    void SetComments(IEnumerable<Comment> comments);
}