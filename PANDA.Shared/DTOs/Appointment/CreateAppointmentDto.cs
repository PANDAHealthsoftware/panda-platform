using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment;
public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    
    public int ClinicianId { get; set; }
    public string Clinician { get; set; } = string.Empty;
    public Department Department { get; init; }
}