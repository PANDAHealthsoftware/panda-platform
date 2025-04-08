using PANDA.Shared.DTOs;

namespace PANDAview.Services;

public interface IAppointmentService
{
    Task<List<AppointmentDto>?> GetAppointmentsAsync();
    Task UpdateAppointmentAsync(UpdateAppointmentDto dto);
}