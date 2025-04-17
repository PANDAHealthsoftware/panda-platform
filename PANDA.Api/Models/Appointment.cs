using PANDA.Domain.Entities;
using PANDA.Shared.Enums;

namespace PANDA.Api.Models;

public class Appointment : AuditableEntity
{
    public int Id { get; set; }

    public int PatientId { get; set; }
    public Patient Patient { get; set; } = default!;

    public DateTime AppointmentDate { get; set; }
    public AppointmentStatus Status { get; set; }

    public int ClinicianId { get; set; }
    public Clinician Clinician { get; set; } = default!;

    public Department Department { get; set; }

    public DateTime? MissedTimestamp { get; set; }
}