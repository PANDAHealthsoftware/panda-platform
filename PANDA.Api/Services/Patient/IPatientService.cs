using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;

namespace PANDA.Api.Services.Patient;

public interface IPatientService
{
    Task<List<PatientSummaryDto>> GetPatientSummariesAsync();
    Task<Models.Patient?> GetPatientByIdAsync(int id);
    Task<List<PatientDto>> GetAllPatientsAsync();
    Task<Models.Patient> AddPatientAsync(CreatePatientDto patientDto);
    Task<bool> UpdatePatientAsync(int id, PatientDto patientDto);
    Task<bool> DeletePatientAsync(int id);
}