using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.Memory;
using Newtonsoft.Json;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Infrastructure.Client.Interfaces;

namespace Tasker.Infrastructure.Client;

public abstract class EmbeddingClient(IMemoryStore memoryStore) : IEmbeddingClient
{
    public IAsyncEnumerable<string> GetCollections()
        => memoryStore.GetCollectionsAsync();
    
    public Task CreateCollection(string collectionName)
        => memoryStore.CreateCollectionAsync(collectionName);

    public async Task<string?> UpsertEmbedding(string collectionName, Embedding<float> embedding, Dictionary<string, object> metadata)
    {
        var id = Guid.NewGuid().ToString();
        var recordMetadata = new MemoryRecordMetadata(
            isReference: true, 
            id: id, 
            description: MetadataProperties.TaskerDataSource,
            text: MetadataProperties.TaskerDataSource,
            externalSourceName: String.Empty, 
            additionalMetadata: JsonConvert.SerializeObject(metadata));
            
        var record = new MemoryRecord(recordMetadata, embedding.Vector, id, new DateTimeOffset(DateTime.Now));
        return await memoryStore.UpsertAsync(collectionName, record);
    }

    public async Task<string[]?> UpsertEmbeddings(string collectionName, GeneratedEmbeddings<Embedding<float>> embeddings, Dictionary<string, object> metadata)
    {
        string?[] ids = [];
        foreach (var embedding in embeddings)
        {
            var id = await UpsertEmbedding(collectionName, embedding, metadata);
            var newIds = ids.Append(id);
            ids = newIds.ToArray();
        }
        
        return ids;
    }

    public IAsyncEnumerable<(MemoryRecord, double)> GetNearestMatches(string collectionName, ReadOnlyMemory<float> embedding, int limit,
        double minRelevanceScore = 0, bool withEmbeddings = false)
        =>  memoryStore.GetNearestMatchesAsync(collectionName, embedding, limit, minRelevanceScore, withEmbeddings);


    public async Task<MemoryRecord?> GetEmbedding(string collectionName, string key, bool withEmbeddings = false)
        => await memoryStore.GetAsync(collectionName, key, withEmbeddings);

    public IAsyncEnumerable<MemoryRecord> GetEmbeddingBatches(string collectionName, string[] keys, bool withEmbeddings = false)
        => memoryStore.GetBatchAsync(collectionName, keys, withEmbeddings);

    public async Task DeleteEmbedding(string collectionName, string key)
        => await memoryStore.RemoveAsync(collectionName, key);
}