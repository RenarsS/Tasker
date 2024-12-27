using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Domain.DTO;
using Tasker.Domain.Processor.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Tasker.Domain.Processor;

public class EmbeddingProcessor(IEmbeddingGenerator<string, Embedding<float>> generator) : IEmbeddingProcessor
{
    public async Task<GeneratedEmbeddings<Embedding<float>>> Process(object entity)
    {
        var values = new []
        {
            JsonConvert.SerializeObject(entity)
        };

        return await generator.GenerateAsync(values);
    }
}