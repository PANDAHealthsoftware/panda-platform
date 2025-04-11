using PANDA.Shared.DTOs.Appointment;

namespace PANDAView.Services.Appointment;

public interface IAppointmentService
{
    Task<List<AppointmentDto>?> GetAppointmentsAsync();
    Task UpdateAppointmentAsync(UpdateAppointmentDto dto);
    Task CreateAppointmentAsync(CreateAppointmentDto dto);
}