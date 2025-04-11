using System.Net.Http.Json;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;

namespace PANDAView.Services.Patient
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _http;

        public PatientService(HttpClient http)
        {
            _http = http;
        }

        public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/patients", dto);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ Patient creation failed: " + content);
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PatientDto>();
        }
        public async Task<List<PatientSummaryDto>> GetPatientSummariesAsync()
        {
            return await _http.GetFromJsonAsync<List<PatientSummaryDto>>("api/patients/summaries")
                   ?? new List<PatientSummaryDto>();
        }
        public async Task<List<PatientDto>> GetPatientsAsync()
        {
            return await _http.GetFromJsonAsync<List<PatientDto>>("api/patients")
                   ?? new List<PatientDto>();
        }

        public async Task<PatientDto?> GetPatientByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<PatientDto>($"api/patients/{id}");
        }
        
        public async Task<PatientDto?> AddPatientAsync(CreatePatientDto newPatient)
        {
            var response = await _http.PostAsJsonAsync("api/patients", newPatient);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<PatientDto>();
        }
        
        public async Task<PatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updateDto)
        {
            var response = await _http.PutAsJsonAsync($"api/patients/{id}", updateDto);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<PatientDto>();
            }

            return null;
        }
    }
}