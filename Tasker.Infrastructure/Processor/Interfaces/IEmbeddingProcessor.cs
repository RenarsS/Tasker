using Microsoft.Extensions.AI;
using Tasker.Domain.DTO;
using Task = Tasker.Domain.DTO.Task;

namespace Tasker.Infrastructure.Processor.Interfaces;

public interface IEmbeddingProcessor
{
    Task<GeneratedEmbeddings<Embedding<float>>> Process(object entity);

    Task<string> ProcessTask(Task task);

    Task<string> ProcessAssignment(Assignment assignment);

    Task<string> ProcessComment(Comment comment);

    System.Threading.Tasks.Task ProcessOrder(Task task, IEnumerable<Assignment> assignment,
        IEnumerable<Comment> comments);
}