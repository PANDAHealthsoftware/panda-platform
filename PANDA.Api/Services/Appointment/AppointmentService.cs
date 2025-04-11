using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.Enums;
using Serilog;

namespace PANDA.Api.Services.Appointment;

public class AppointmentService : IAppointmentService
{
    private readonly PandaDbContext _context;
    private readonly IMapper _mapper;

    public AppointmentService(PandaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UpdateAppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        if (dto.PatientId == 0 || string.IsNullOrWhiteSpace(dto.PatientName))
        {
            throw new ArgumentException("Invalid appointment data: patient info is required");
        }
        
        var clash = await _context.Appointments
            .AnyAsync(a => a.ClinicianId == dto.ClinicianId
                           && a.AppointmentDate == dto.AppointmentDate
                           && a.Status != AppointmentStatus.Cancelled);
        
        if (clash)
        {
            Log.Warning("Double-booking attempt: Clinician {ClinicianId} at {Time}", dto.ClinicianId, dto.AppointmentDate);
            throw new InvalidOperationException("This clinician is already booked at the selected time.");
        }
        
        var appointment = _mapper.Map<Models.Appointment>(dto);
        appointment.AppointmentDate = appointment.AppointmentDate;
        appointment.ClinicianId = dto.ClinicianId;
        appointment.Clinician = dto.Clinician;
        appointment.PatientName = dto.PatientName;
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync();
        return _mapper.Map<UpdateAppointmentDto>(appointment);
    }

    public async Task<UpdateAppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _context.Appointments.FindAsync(id);
        return appointment == null ? null : _mapper.Map<UpdateAppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _context.Appointments
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();
        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
    {
        if (dto.PatientId == 0 || string.IsNullOrWhiteSpace(dto.PatientName))
        {
            throw new ArgumentException("Invalid appointment data: patient info is required");
        }
        
        var clash = await _context.Appointments
            .AnyAsync(a => a.ClinicianId == dto.ClinicianId
                           && a.AppointmentDate == dto.AppointmentDate
                           && a.Status != AppointmentStatus.Cancelled);
        
        if (clash)
        {
            Log.Warning("Double-booking attempt: Clinician {ClinicianId} at {Time}", dto.ClinicianId, dto.AppointmentDate);
            throw new InvalidOperationException("This clinician is already booked at the selected time.");
        }
        
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
        appointment.ClinicianId = dto.ClinicianId;
        appointment.PatientName = dto.PatientName;

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
            appointment.MissedTimestamp = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            Log.Information(LogMessages.AppointmentMarkedMissed, appointmentId);
        }
    }
}
