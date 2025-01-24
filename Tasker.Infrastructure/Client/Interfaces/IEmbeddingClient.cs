using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Memory;

namespace Tasker.Infrastructure.Client.Interfaces;

public interface IEmbeddingClient
{
    IAsyncEnumerable<string> GetCollections();
    
    Task CreateCollection(string collectionName);
    
    Task<string?> UpsertEmbedding(string collectionName, Embedding<float> embedding, Dictionary<string, object> metadata);
    
    Task<string[]?> UpsertEmbeddings(string collectionName, GeneratedEmbeddings<Embedding<float>> embeddings, Dictionary<string, object> metadata);
    
    IAsyncEnumerable<ValueTuple<MemoryRecord, double>> GetNearestMatches(string collectionName, ReadOnlyMemory<float> embedding, int limit, double minRelevanceScore = 0, bool withEmbeddings = false);
    
    Task<MemoryRecord?> GetEmbedding(string collectionName, string key, bool withEmbeddings = false);
    
    IAsyncEnumerable<MemoryRecord> GetEmbeddingBatches(string collectionName, string[] keys, bool withEmbeddings = false);
    
    Task DeleteEmbedding(string collectionName, string key);
}