﻿using PANDA.Shared.Enums;

namespace PANDA.Shared.DTOs.Appointment;

public class AppointmentDto
{
    public int Id { get; init; }

    public int PatientId { get; init; }
    public string PatientFullName { get; init; } = string.Empty;

    public DateTime AppointmentDate { get; init; }
    public AppointmentStatus Status { get; init; }

    public int ClinicianId { get; init; }
    public string ClinicianFullName { get; init; } = string.Empty;

    public Department Department { get; init; }

    public string? Reason { get; init; } // ⬅️ Add this
}