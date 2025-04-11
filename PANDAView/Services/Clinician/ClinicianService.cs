using System.Net.Http.Json;
using PANDA.Shared.DTOs.Clinician;

namespace PANDAView.Services.Clinician;

public class ClinicianService : IClinicianService
{
    private readonly HttpClient _http;

    public ClinicianService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<ClinicianDto>> GetCliniciansAsync()
    {
        var result = await _http.GetFromJsonAsync<List<ClinicianDto>>("api/clinicians");
        return result ?? new List<ClinicianDto>();
    }
}