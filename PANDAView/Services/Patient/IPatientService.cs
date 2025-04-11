using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;

namespace PANDAView.Services.Patient;

public interface IPatientService
{
    Task<PatientDto> CreatePatientAsync(CreatePatientDto dto);
    Task<UpdatePatientDto?> UpdatePatientAsync(int id, UpdatePatientDto updateDto);
    Task<List<PatientSummaryDto>> GetPatientSummariesAsync();
    Task<List<PatientDto>> GetPatientsAsync();
    Task<PatientDto?> GetPatientByIdAsync(int id);
    Task<PatientDto?> AddPatientAsync(CreatePatientDto newPatient);
}