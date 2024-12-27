using AutoMapper;
using Tasker.Domain.DTO;
using Tasker.Infrastructure.Builders.Interfaces;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Builders;

public class OrderBuilder(IMapper mapper) : IOrderBuilder
{
    private Order _order = new Order
    {
        OrderId = new Guid(),
        Title = string.Empty,
        Description = string.Empty,
        Status = 0,
        CreatedBy = 0
    };
    
    public Order Build() => _order;

    public void Reset()
    {
        _order = new Order
        {
            OrderId = new Guid(),
            Title = string.Empty,
            Description = string.Empty,
            Status = 0,
            CreatedBy = 0
        };
    }

    public void SetTask(Task task)
    {
        _order = mapper.Map<Order>(task);
    }

    public void SetAssignments(IEnumerable<Assignment> assignments)
    {
        _order.Assignments = assignments;
    }

    public void SetComments(IEnumerable<Comment> comments)
    {
        _order.Comments = comments;
    }
}