using PANDA.Api.Dto;
using PANDA.Api.Models;
using PANDA.Shared.DTOs;

namespace PANDA.Api.Services;

public interface IAppointmentService
{
    Task<UpdateAppointmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<UpdateAppointmentDto>> GetAllAsync();
    Task<UpdateAppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateAsync(int id, UpdateAppointmentDto updateAppointmentDto);
    Task<bool> DeleteAsync(int id);
    Task TrackMissedAppointmentsAsync(int appointmentId);
}