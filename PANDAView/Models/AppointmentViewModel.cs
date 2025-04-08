using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Enums;

namespace PANDAView.Models;

public class AppointmentViewModel
{
    public int Id { get; set; }
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Clinician is required")]
    public string Clinician { get; set; } = string.Empty;

    [Required(ErrorMessage = "Appointment date is required")]
    public DateTime AppointmentDate { get; set; }

    public Department Department { get; set; }
    public AppointmentStatus Status { get; set; }

    // Optional: Allow custom durations later
    public int DurationMinutes { get; set; } = 30;

    public DateTime AppointmentEnd => AppointmentDate.AddMinutes(DurationMinutes);
}
