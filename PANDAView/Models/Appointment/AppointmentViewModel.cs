using PANDA.Shared.Enums;

namespace PANDAView.Models.Appointment;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public int ClinicianId { get; set; }
    public string Clinician { get; set; } = string.Empty;

    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public Department Department { get; set; }

    public int DurationMinutes { get; set; } = 30;
    public DateTime AppointmentEnd => AppointmentDate.AddMinutes(DurationMinutes);

    public string DisplayLabel => $"{PatientName} with {Clinician}";
}
