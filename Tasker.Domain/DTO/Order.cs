namespace Tasker.Domain.DTO;

public class Order : Task
{
    public required Guid OrderId { get; set; }
    
    public IEnumerable<Assignment>? Assignments { get; set; }

    public IEnumerable<Comment>? Comments { get; set; }
}