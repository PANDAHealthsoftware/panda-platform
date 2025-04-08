using System.Net.Http.Json;
using PANDA.Shared.DTOs;

namespace PANDAview.Services;

public class AppointmentService: IAppointmentService
{
    private readonly HttpClient _http;

    public AppointmentService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<AppointmentDto>?> GetAppointmentsAsync()
        => await _http.GetFromJsonAsync<List<AppointmentDto>>("api/appointments");

    public async Task UpdateAppointmentAsync(UpdateAppointmentDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/appointments/{dto.Id}", dto);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Failed to update appointment: {response.StatusCode} - {error}");
        }
    }
}