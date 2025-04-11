using PANDA.Shared.DTOs.Clinician;

namespace PANDAView.Services.Clinician;

public interface IClinicianService
{
    Task<List<ClinicianDto>> GetCliniciansAsync();
}