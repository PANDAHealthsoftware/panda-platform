using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using PANDA.Api.Services.Audit;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.Enums;
using Serilog;

namespace PANDA.Api.Services.Appointment;

public class AppointmentService : IAppointmentService
{
    private readonly PandaDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAuditService _auditService;

    public AppointmentService(PandaDbContext context, IMapper mapper, IAuditService auditService)
    {
        _context = context;
        _mapper = mapper;
        _auditService = auditService;
    }

    public async Task<UpdateAppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        if (dto.PatientId == 0 || dto.ClinicianId == 0)
        {
            throw new ArgumentException("Invalid appointment data: patient and clinician are required.");
        }

        var clash = await _context.Appointments.AnyAsync(a =>
            a.ClinicianId == dto.ClinicianId &&
            a.AppointmentDate == dto.AppointmentDate &&
            a.Status != AppointmentStatus.Cancelled);

        if (clash)
        {
            Log.Warning("Double-booking attempt: Clinician {ClinicianId} at {Time}", dto.ClinicianId, dto.AppointmentDate);
            throw new InvalidOperationException("This clinician is already booked at the selected time.");
        }

        var appointment = _mapper.Map<Models.Appointment>(dto);
        appointment.Clinician = await _context.Clinicians.FindAsync(dto.ClinicianId)
            ?? throw new ArgumentException("Clinician not found");

        _context.Appointments.Add(appointment);
        
        await _context.Appointments.AddAsync(appointment);
        await _auditService.LogCreateAsync(dto, appointment);
        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateAppointmentDto>(appointment);
    }

    public async Task<UpdateAppointmentDto?> GetByIdAsync(int id)
    {
        var appointment = await _context.Appointments
            .Include(a => a.Clinician)
            .Include(a => a.Patient)
            .FirstOrDefaultAsync(a => a.Id == id);

        return appointment == null ? null : _mapper.Map<UpdateAppointmentDto>(appointment);
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        var appointments = await _context.Appointments
            .Include(a => a.Clinician)
            .Include(a => a.Patient)
            .OrderBy(a => a.AppointmentDate)
            .ToListAsync();

        return _mapper.Map<List<AppointmentDto>>(appointments);
    }

    public async Task<bool> UpdateAppointmentAsync(int id, UpdateAppointmentDto dto)
    {
        if (dto.PatientId == 0 || dto.ClinicianId == 0)
        {
            throw new ArgumentException("Invalid appointment data: patient and clinician are required.");
        }

        var clash = await _context.Appointments.AnyAsync(a =>
            a.Id != id &&
            a.ClinicianId == dto.ClinicianId &&
            a.AppointmentDate == dto.AppointmentDate &&
            a.Status != AppointmentStatus.Cancelled);

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

        // Deep clone original for audit snapshot
        var original = JsonConvert.DeserializeObject<Models.Appointment>(
            JsonConvert.SerializeObject(appointment)); // deep copy for change tracking

        // Apply updates
        appointment.PatientId = dto.PatientId;
        appointment.AppointmentDate = dto.AppointmentDate;
        appointment.Status = dto.Status;
        appointment.ClinicianId = dto.ClinicianId;
        appointment.Department = dto.Department;
        appointment.Clinician = await _context.Clinicians.FindAsync(dto.ClinicianId)
                                ?? throw new ArgumentException("Clinician not found");

        // 🔍 Audit the changes
        await _auditService.LogUpdateAsync(dto, original, appointment);

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

        if (appointment.AppointmentDate < DateTime.UtcNow)
        {
            appointment.Status = AppointmentStatus.Missed;
            appointment.MissedTimestamp = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            Log.Information(LogMessages.AppointmentMarkedMissed, appointmentId);
        }
    }
}
