namespace PANDA.Api.Services.OpenAi;

public interface IOpenAiService
{
    Task<string> DetectIntent(string prompt);
    Task<string> GetChatCompletion(string prompt);
}