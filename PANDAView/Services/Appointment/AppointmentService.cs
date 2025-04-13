using System.Net.Http.Json;
using PANDA.Shared.DTOs.Appointment;
using PANDAView.Services.Appointment;

public class AppointmentService : IAppointmentService
{
    private readonly HttpClient _http;

    public AppointmentService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<AppointmentDto>?> GetAppointmentsAsync()
    {
        return await _http.GetFromJsonAsync<List<AppointmentDto>>("api/appointments");
    }

    public async Task UpdateAppointmentAsync(UpdateAppointmentDto dto)
    {
        await _http.PutAsJsonAsync($"api/appointments/{dto.Id}", dto);
    }

    public async Task CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        await _http.PostAsJsonAsync("api/appointments", dto); // ✅ POST to API
    }
}