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

    // Get all clinicians
    public async Task<List<ClinicianDto>> GetCliniciansAsync()
    {
        try
        {
            var result = await _http.GetFromJsonAsync<List<ClinicianDto>>("api/clinicians");
            return result ?? new List<ClinicianDto>();
        }
        catch (Exception ex)
        {
            // Log error (replace with your logging)
            Console.Error.WriteLine($"Failed to load clinicians: {ex.Message}");
            // Optionally rethrow or return empty
            throw new Exception("Could not retrieve clinicians from server.", ex);
        }
    }

    // Get single clinician by ID
    public async Task<ClinicianDto?> GetClinicianByIdAsync(int id)
    {
        try
        {
            var result = await _http.GetFromJsonAsync<ClinicianDto>($"api/clinicians/{id}");
            return result;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load clinician: {ex.Message}");
            return null; // Or throw, if that's your policy
        }
    }

    // Add a new clinician
    public async Task<bool> AddClinicianAsync(CreateClinicianDto dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("api/clinicians", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"Failed to add clinician: {error}");
                throw new Exception($"Failed to add clinician: {error}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception adding clinician: {ex.Message}");
            throw new Exception("Could not add clinician.", ex);
        }
    }

    // Update an existing clinician
    public async Task<bool> UpdateClinicianAsync(int id, UpdateClinicianDto dto)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"api/clinicians/{id}", dto);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"Failed to update clinician: {error}");
                throw new Exception($"Failed to update clinician: {error}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception updating clinician: {ex.Message}");
            throw new Exception("Could not update clinician.", ex);
        }
    }

    // Delete a clinician
    public async Task<bool> DeleteClinicianAsync(int id)
    {
        try
        {
            var response = await _http.DeleteAsync($"api/clinicians/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.Error.WriteLine($"Failed to delete clinician: {error}");
                throw new Exception($"Failed to delete clinician: {error}");
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception deleting clinician: {ex.Message}");
            throw new Exception("Could not delete clinician.", ex);
        }
    }
}
