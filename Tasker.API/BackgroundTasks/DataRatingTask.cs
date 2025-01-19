using Tasker.API.Services.Interfaces;

namespace Tasker.API.BackgroundTasks;

public class DataRatingTask(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var analyticsService = scope.ServiceProvider.GetService<IAnalyticsService>();
            
        await  analyticsService?.RateQueryResponses()!;
        await analyticsService?.RateResponseRetrievals()!;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}