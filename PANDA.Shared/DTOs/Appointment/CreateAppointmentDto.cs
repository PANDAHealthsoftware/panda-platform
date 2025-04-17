using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment;
public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public int ClinicianId { get; set; }
    public Department Department { get; init; }
}