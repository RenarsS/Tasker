using AutoMapper;
using Tasker.Domain.DTO;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Infrastructure.Mapping;

public class TaskerProfile : Profile
{
    public TaskerProfile()
    {
        CreateMap<Task, Order>().ReverseMap();
    }
}