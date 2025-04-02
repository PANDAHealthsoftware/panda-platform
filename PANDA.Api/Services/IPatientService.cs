namespace PANDA.Api.Services;

public interface IPatientService
{
    Task<int> AddPatientAsync(Patient patient);
    Task<Patient?> GetPatientByIdAsync(int id);
    Task<IEnumerable<Patient>> GetAllPatientsAsync();
    Task<bool> UpdatePatientAsync(Patient patient);
    Task<bool> DeletePatientAsync(int id);
}