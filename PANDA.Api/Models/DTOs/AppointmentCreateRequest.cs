﻿namespace PANDA.Api.Models.DTOs;

public class AppointmentCreateRequest
{
    public Guid PatientId { get; set; }
    public DateTime DateTime { get; set; }
    public required string Reason { get; set; }
}