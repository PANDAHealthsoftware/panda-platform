using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Enums;

namespace PANDAView.Models.Appointment;

public class EditAppointmentModel
{
    public int Id { get; set; }

    [Required]
    public int PatientId { get; set; }

    public string? PatientName { get; set; }

    [Required]
    public int ClinicianId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    public Department Department { get; set; }
    public AppointmentStatus Status { get; set; }

    public int DurationMinutes { get; set; } = 30;
}