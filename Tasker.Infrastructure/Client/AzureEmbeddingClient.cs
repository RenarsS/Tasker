using Microsoft.SemanticKernel.Memory;

namespace Tasker.Infrastructure.Client;

public class AzureEmbeddingClient(IMemoryStore memoryStore) : EmbeddingClient(memoryStore) { }