using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.Chroma;
using Tasker.Infrastructure.Client.Interfaces;

namespace Tasker.Infrastructure.Client;

public class ChromaEmbeddingClient(IConfiguration configuration, ILoggerFactory loggerFactory)
    : IEmbeddingClient
{
    private readonly IChromaClient _chromaClient = new ChromaClient(configuration["ChromaDb:ConnectionString"] ?? string.Empty, loggerFactory);

    public IAsyncEnumerable<string> GetCollections()
        => _chromaClient.ListCollectionsAsync();

    public async Task CreateCollection(string collectionName)
        => await _chromaClient.CreateCollectionAsync(collectionName);

    public Task<string?> UpsertEmbedding(string collectionName, Embedding<float> embedding, Dictionary<string, object> metadata)
    {
        throw new NotImplementedException();
    }

    public async Task UpsertEmbedding(string collectionName, string[] embeddingIds, ReadOnlyMemory<float>[] embeddings, object[] metadata)
        => await _chromaClient.UpsertEmbeddingsAsync(collectionName, embeddingIds, embeddings, metadata);

    public async Task<string[]?> UpsertEmbeddings(string collectionName, GeneratedEmbeddings<Embedding<float>> embeddings, Dictionary<string, object> metadata)
    {
        string[] ids = [];
        foreach (var embedding in embeddings)
        {
            var result= ids.Append(Guid.NewGuid().ToString());
            ReadOnlyMemory<float>[] vectors = [embedding.Vector];
            object[] meta = [metadata.ToArray()];
            await UpsertEmbedding(collectionName,ids, vectors, meta);
        }

        return ids;
    }
}