using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace PANDAView.Services.HttpHandlers;
public class AuthTokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthTokenHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorage.GetItemAsync<string>("auth_token");
        Console.WriteLine($"[AuthHandler] Token: {token}");
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine($"[AuthHandler] Attached token: {token[..Math.Min(30, token.Length)]}...");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}