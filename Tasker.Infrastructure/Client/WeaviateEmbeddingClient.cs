using Microsoft.SemanticKernel.Memory;

namespace Tasker.Infrastructure.Client;

public class WeaviateEmbeddingClient(IMemoryStore memoryStore) : EmbeddingClient(memoryStore) { }