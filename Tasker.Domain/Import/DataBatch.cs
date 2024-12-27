using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Domain.Import;

public class DataBatch
{
    public Task Task { get; set; }

    public IEnumerable<Assignment> Assignments { get; set; }

    public IEnumerable<Comment> Comments { get; set; }
}