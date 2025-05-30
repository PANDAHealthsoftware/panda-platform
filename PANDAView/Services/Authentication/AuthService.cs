using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using PANDAView.Models;

namespace PANDAView.Services.Authentication;

public class AuthService : IAuthService
{
    public const string TokenKey = "auth_token";

    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authProvider;

    public AuthService(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authProvider)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _authProvider = authProvider;
    }

    public async Task<bool> LoginAsync(LoginModel credentials)
    {
        var content = new StringContent(JsonSerializer.Serialize(credentials), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("api/auth/login", content);
        if (!response.IsSuccessStatusCode)
            return false;

        var responseBody = await response.Content.ReadAsStringAsync();
        var token = JsonSerializer.Deserialize<TokenResponse>(responseBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })?.AccessToken;

        if (string.IsNullOrWhiteSpace(token))
            return false;

        await _localStorage.SetItemAsync(TokenKey, token);
        

        if (_authProvider is CustomAuthProvider customAuth)
            customAuth.NotifyAuthenticationStateChanged();

        return true;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(TokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;

        if (_authProvider is CustomAuthProvider customAuth)
            customAuth.NotifyAuthenticationStateChanged();
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>(TokenKey);
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return false;

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwt.ValidTo > DateTime.UtcNow;
    }

    public async Task<string?> GetCurrentUserRoleAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return null;

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        return jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
    }
    
    public async Task<List<string>> GetCurrentUserRolesAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrEmpty(token)) return new List<string>();

        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        // There may be multiple role claims
        return jwt.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .ToList();
    }
}
