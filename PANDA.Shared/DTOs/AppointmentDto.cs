using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs;

public class AppointmentDto
{
    public int Id { get; init; }
    public int PatientId { get; init; }
    public DateTime AppointmentDate { get; init; }

    public AppointmentStatus Status { get; init; }
    public string Clinician { get; init; } = string.Empty;
    public Department Department { get; init; }
}