using Microsoft.Extensions.AI;

namespace Tasker.Infrastructure.Client.Interfaces;

public interface IEmbeddingClient
{
    IAsyncEnumerable<string> GetCollections();
    
    Task CreateCollection(string collectionName);
    
    Task UpsertEmbedding(string collectionName, string[] embeddingIds, ReadOnlyMemory<float>[] embeddings, object[] metadata);
    
    Task UpsertEmbedding(string collectionName, GeneratedEmbeddings<Embedding<float>> embeddings, Dictionary<string, object> metadata);
}