using System.Net.Http.Json;
using PANDAView.Services.Chat;

namespace PANDAView.Pages.Chat;

public class ChatbotService : IChatbotService
{
    private readonly HttpClient _httpClient;

    public ChatbotService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendMessageAsync(string userMessage)
    {
        var response = await _httpClient.PostAsJsonAsync("api/chat", userMessage);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return "Oops! Something went wrong.";
    }
}