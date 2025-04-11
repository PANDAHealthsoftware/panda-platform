using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment;

public class AppointmentDto
{
    public int Id { get; init; }
    public int PatientId { get; init; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; init; }

    public AppointmentStatus Status { get; init; }
    
    public int ClinicianId { get; set; }
    public string Clinician { get; init; } = string.Empty;
    public Department Department { get; init; }
}