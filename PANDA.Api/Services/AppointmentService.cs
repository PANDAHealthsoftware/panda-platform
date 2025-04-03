using Microsoft.EntityFrameworkCore;
using PANDA.Api.Common;
using PANDA.Api.Dto;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using AutoMapper;
using Serilog;

namespace PANDA.Api.Services;

public class AppointmentService : IAppointmentService
{
    private readonly PandaDbContext _context;
    private readonly IMapper _mapper;

    public AppointmentService(PandaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        var appointment = _mapper.Map<Appointment>(dto);
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _context.Appointments.ToListAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<bool> UpdateAsync(int id, AppointmentDto dto)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null || appointment.Status == AppointmentStatus.Cancelled)
        {
            Log.Warning(LogMessages.UpdateCancelledBlocked, id);
            return false;
        }

        appointment.PatientId = dto.PatientId;
        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.Status = dto.Status;
        appointment.Clinician = dto.Clinician;
        appointment.Department = dto.Department;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        if (appointment == null)
        {
            Log.Warning(LogMessages.AppointmentNotFoundForDelete, id);
            return false;
        }

        Log.Information(LogMessages.AppointmentCancelling, id);
        appointment.Status = AppointmentStatus.Cancelled;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task TrackMissedAppointmentsAsync(int appointmentId)
    {
        var appointment = await _context.Appointments.FindAsync(appointmentId);
        if (appointment == null || appointment.Status == AppointmentStatus.Cancelled)
        {
            Log.Information(LogMessages.SkipMissedTrackingCancelled, appointmentId);
            return;
        }

        if (appointment.AppointmentDate < DateTimeOffset.UtcNow)
        {
            appointment.Status = AppointmentStatus.Missed;
            appointment.MissedTimestamp = DateTimeOffset.UtcNow;
            await _context.SaveChangesAsync();
            Log.Information(LogMessages.AppointmentMarkedMissed, appointmentId);
        }
    }
}
