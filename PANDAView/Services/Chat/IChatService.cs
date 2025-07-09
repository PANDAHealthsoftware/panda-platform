namespace PANDAView.Services.Chat;

public interface IChatbotService
{
    Task<string> SendMessageAsync(string userMessage);
}