using PANDA.Api.Dto;
using PANDA.Api.Models;

namespace PANDA.Api.Services;

public interface IPatientService
{ 
    Task<Patient> GetPatientByIdAsync(int id);
    Task<List<PatientDto>> GetAllPatientsAsync();
    Task<Patient> AddPatientAsync(CreatePatientDto patientDto);
    Task<bool> UpdatePatientAsync(int id, PatientDto patientDto);
    Task<bool> DeletePatientAsync(int id);
}