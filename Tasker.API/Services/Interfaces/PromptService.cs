using StackExchange.Redis;

namespace Tasker.API.Services.Interfaces;

public class PromptService(IConfiguration configuration, IConnectionMultiplexer multiplexer) : IPromptService
{
    private const string PromptSection = "Prompts:";

    private readonly IDatabase _redis = multiplexer.GetDatabase();
    
    public async Task<string> GetPrompt(string promptName)
    {
        string? prompt = await _redis.StringGetAsync(promptName);
        if (string.IsNullOrEmpty(prompt))
        {
            prompt = await GetPromptFromFile(promptName);
            await _redis.StringSetAsync(promptName, prompt);
            await _redis.KeyExpireAsync(promptName, TimeSpan.FromHours(1));
        }
        
        return prompt;
    }

    private async Task<string> GetPromptFromFile(string promptName)
    {
        try
        {
            var promptPath = configuration[PromptSection + promptName];
            var prompt = await File.ReadAllTextAsync(promptPath);
            return prompt;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}