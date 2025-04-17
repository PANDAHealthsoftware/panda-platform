using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Enums;

namespace PANDAView.Models.Appointment;

public class EditAppointmentModel
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int ClinicianId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public Department Department { get; set; }
}