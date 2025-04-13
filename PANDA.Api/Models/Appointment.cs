using System.Text.Json.Serialization;
using PANDA.Shared.Converters;
using PANDA.Shared.Enums;

namespace PANDA.Api.Models;

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientName { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }
    public int ClinicianId { get; set; }
    public required string Clinician { get; set; }
    public Department Department { get; set; }
    public DateTime? MissedTimestamp { get; set; } 
}