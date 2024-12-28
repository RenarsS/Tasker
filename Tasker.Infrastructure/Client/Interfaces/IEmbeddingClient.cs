using Microsoft.Extensions.AI;

namespace Tasker.Infrastructure.Client.Interfaces;

public interface IEmbeddingClient
{
    IAsyncEnumerable<string> GetCollections();
    
    Task CreateCollection(string collectionName);
    
    Task<string?> UpsertEmbedding(string collectionName, Embedding<float> embedding, Dictionary<string, object> metadata);
    
    Task<string[]?> UpsertEmbeddings(string collectionName, GeneratedEmbeddings<Embedding<float>> embeddings, Dictionary<string, object> metadata);
}