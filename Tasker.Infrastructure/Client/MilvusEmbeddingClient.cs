using Microsoft.SemanticKernel.Connectors.Milvus;

namespace Tasker.Infrastructure.Client;

public class MilvusEmbeddingClient(MilvusMemoryStore memoryStore) : EmbeddingClient(memoryStore) { }