using PANDA.Shared.DTOs.Clinician;

namespace PANDAView.Services.Clinician;

public interface IClinicianService
{
    Task<List<ClinicianDto>> GetCliniciansAsync();
    Task<ClinicianDto?> GetClinicianByIdAsync(int id);
    Task<bool> AddClinicianAsync(CreateClinicianDto dto);
    Task<bool> UpdateClinicianAsync(int id, UpdateClinicianDto dto);
    Task<bool> DeleteClinicianAsync(int id);
    
}