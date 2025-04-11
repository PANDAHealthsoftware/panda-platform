using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;
using PANDAView.Helpers;

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
                Console.WriteLine("❌ Patient creation failed: " + content); // For dev log

                // Attempt to parse validation errors
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest &&
                    response.Content.Headers.ContentType?.MediaType == "application/problem+json")
                {
                    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    throw new ApiValidationException(problemDetails);
                }

                // For non-validation errors
                throw new HttpRequestException($"API returned {response.StatusCode}: {content}");
            }

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
        
        public async Task<UpdatePatientDto?> UpdatePatientAsync(int id, UpdatePatientDto dto)
        {
            var response = await _http.PutAsJsonAsync($"api/patients/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.BadRequest &&
                    response.Content.Headers.ContentType?.MediaType == "application/problem+json")
                {
                    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
                    throw new ApiValidationException(problemDetails);
                }

                throw new HttpRequestException($"Update failed with status code {response.StatusCode}: {content}");
            }

            return dto;
        }
    }
}