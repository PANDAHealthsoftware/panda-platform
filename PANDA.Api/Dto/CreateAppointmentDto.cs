using PANDA.Api.Common;

namespace PANDA.Api.Dto;
public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public DateTimeOffset AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;

    public string Clinician { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}