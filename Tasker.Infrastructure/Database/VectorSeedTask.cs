using Microsoft.Extensions.Configuration;
using Tasker.Infrastructure.Client;
using Tasker.Infrastructure.Client.Interfaces;
using Tasker.Infrastructure.Database.Interface;

namespace Tasker.Infrastructure.Database;


public class VectorSeedTask(IConfiguration configuration, IEmbeddingClient embeddingClient) : IVectorSeedTask
{
    public async Task Execute()
    {
        var collections = configuration.GetSection("VectorDb:Collections").Get<string[]>();
        if (collections is null)
        {
            return;
        }

        var collectionsAvailable = embeddingClient.GetCollections().ToBlockingEnumerable();
        var missingCollections = collections.Where(c => !collectionsAvailable.Contains(c));

        foreach (var collection in missingCollections)
        {
            await embeddingClient.CreateCollection(collection);
        }
    }
}