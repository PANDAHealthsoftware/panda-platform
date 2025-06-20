﻿using PANDA.Shared.DTOs.Appointment;

namespace PANDA.Api.Services.Appointment;

public interface IAppointmentService
{
    Task<UpdateAppointmentDto?> GetByIdAsync(int id);
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<UpdateAppointmentDto> CreateAsync(CreateAppointmentDto dto);
    Task<bool> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto);
    Task<bool> DeleteAsync(int id);
    Task TrackMissedAppointmentsAsync(int appointmentId);
}