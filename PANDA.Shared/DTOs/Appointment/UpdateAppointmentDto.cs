using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment;

public class UpdateAppointmentDto
{
    public int Id { get; set; } // Required for update
    public int PatientId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public int ClinicianId { get; set; }
    public Department Department { get; set; }
    public string? Reason { get; set; }
}