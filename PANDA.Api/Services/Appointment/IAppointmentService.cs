using PANDA.Shared.DTOs.Appointment;

namespace PANDA.Api.Services.Appointment;

public interface IAppointmentService
{
    Task<UpdateAppointmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<UpdateAppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
    Task TrackMissedAppointmentsAsync(int appointmentId);
    Task<List<AppointmentDto>> GetAppointmentsForTodayAsync(int userId);
    Task<List<AppointmentDto>> GetAppointmentsInRangeAsync(int userId, DateTime start, DateTime end);
    Task<bool> IsPatientAsync(int userId);
    Task<bool> IsClinicianAsync(int userId);
}