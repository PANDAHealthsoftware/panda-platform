using PANDA.Api.Dto;
using PANDA.Api.Models;

namespace PANDA.Api.Services;

public interface IAppointmentService
{
    Task<AppointmentDto> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateAsync(int id, AppointmentDto appointmentDto);
    Task<bool> DeleteAsync(int id);
    Task TrackMissedAppointmentsAsync(int appointmentId);
}