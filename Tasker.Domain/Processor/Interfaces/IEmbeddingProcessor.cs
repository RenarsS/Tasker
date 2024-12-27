using Microsoft.Extensions.AI;
using Tasker.Domain.DTO;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Domain.Processor.Interfaces;

public interface IEmbeddingProcessor
{
    Task<GeneratedEmbeddings<Embedding<float>>> Process(object entity);
}