using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Clinician;

namespace PANDA.Api.Services.Clinician;

public interface IClinicianService
{
    Task<IEnumerable<ClinicianDto>> GetAllAsync();
    Task<ClinicianDto?> GetByIdAsync(int id);
    Task<ClinicianDto> CreateAsync(CreateClinicianDto dto);
}