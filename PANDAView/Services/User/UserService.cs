using System.Net.Http.Json;
using PANDA.Shared.DTOs.User;

namespace PANDAView.Services.User;

public class UserService : IUserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _http.GetFromJsonAsync<List<UserDto>>("api/users");
            return users ?? new List<UserDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading users: {ex.Message}");
            return new List<UserDto>();
        }
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<UserDto>($"api/users/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user {id}: {ex.Message}");
            return null;
        }
    }

    public async Task CreateUserAsync(CreateUserDto user)
    {
        var response = await _http.PostAsJsonAsync("api/users", user);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Create failed: {error}");
        }
    }

    public async Task UpdateUserAsync(int id, UpdateUserDto user)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", user);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Update failed: {error}");
        }
    }

    public async Task DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Delete failed: {error}");
        }
    }
}