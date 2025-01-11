namespace Tasker.API.Services.Interfaces;

public interface IPromptService
{
    Task<string> GetPrompt(string promptName);
}