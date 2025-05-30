using System.Net.Http.Json;
using PANDA.Shared.DTOs.Appointment;

namespace PANDAView.Services.Appointment;

/// <summary>
/// Provides methods for managing appointments, including retrieving, updating, and creating appointments.
/// </summary>
public class AppointmentService : IAppointmentService
{
    /// <summary>
    /// Represents the instance of <see cref="HttpClient"/> used to send HTTP requests
    /// and receive HTTP responses from the API endpoints in the <see cref="AppointmentService"/>.
    /// </summary>
    private readonly HttpClient _http;

    /// Provides methods for managing appointment data and interacting with the backend API for operations
    /// such as retrieving appointments, updating appointment details, and creating new appointments.
    public AppointmentService(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Retrieves a list of appointments asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <c>AppointmentDto</c> objects, or null if no data is available.</returns>
    public async Task<List<AppointmentDto>?> GetAppointmentsAsync()
    {
        return await _http.GetFromJsonAsync<List<AppointmentDto>>("api/appointments");
    }

    /// <summary>
    /// Updates an existing appointment with the provided details.
    /// </summary>
    /// <param name="dto">The data transfer object containing information about the appointment to be updated.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task UpdateAppointmentAsync(UpdateAppointmentDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/appointments/{dto.Id}", dto);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to update appointment: {message}");
        }
    }

    /// <summary>
    /// Sends a request to create a new appointment.
    /// </summary>
    /// <param name="dto">The details of the appointment to be created, including patient information, clinician, date, department, and reason.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task CreateAppointmentAsync(CreateAppointmentDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/appointments", dto);
        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create appointment: {message}");
        }
    }
}