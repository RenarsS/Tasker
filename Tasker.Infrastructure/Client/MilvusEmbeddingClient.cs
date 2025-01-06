using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel.Connectors.Milvus;
using Microsoft.SemanticKernel.Memory;
using Newtonsoft.Json;
using Tasker.Domain.Constants.Embeddings;
using Tasker.Infrastructure.Client.Interfaces;

namespace Tasker.Infrastructure.Client;

public class MilvusEmbeddingClient(IConfiguration configuration) : IEmbeddingClient
{
    private readonly MilvusMemoryStore _milvusMemoryStore = new(configuration["MilvusDb:Host"]!);

    public IAsyncEnumerable<string> GetCollections()
        =>  _milvusMemoryStore.GetCollectionsAsync();

    public async Task CreateCollection(string collectionName)
        => await _milvusMemoryStore.CreateCollectionAsync(collectionName);

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
        return await _milvusMemoryStore.UpsertAsync(collectionName, record);
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

    public IAsyncEnumerable<(MemoryRecord, double)> GetNearestMatches(string collectionName, ReadOnlyMemory<float> embedding, int limit, double minRelevanceScore = 0,
        bool withEmbeddings = false)
        =>  _milvusMemoryStore.GetNearestMatchesAsync(collectionName, embedding, limit, minRelevanceScore, withEmbeddings);

    public async Task<MemoryRecord?> GetEmbedding(string collectionName, string key, bool withEmbeddings = false)
        => await _milvusMemoryStore.GetAsync(collectionName, key, withEmbeddings);
}