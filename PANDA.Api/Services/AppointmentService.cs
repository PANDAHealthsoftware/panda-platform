using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Common;
using PANDA.Api.Dto;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using Serilog;

namespace PANDA.Api.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly PandaDbContext _context;
        private readonly IMapper _mapper;

        public AppointmentService(PandaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppointmentDto> GetByIdAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) throw new InvalidOperationException("Appointment not found");

            return _mapper.Map<AppointmentDto>(appointment);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _context.Appointments.ToListAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var appointmentDto = _mapper.Map<AppointmentDto>(appointment); 
            return appointmentDto;
        }
        
        public async Task<bool> UpdateAsync(int id, AppointmentDto appointmentDto)
        {
            var existing = await _context.Appointments.FindAsync(id);
            if (existing == null)
                return false;

            if (existing.Status == AppointmentStatus.Cancelled)
            {
                Log.Warning("Attempted to update a cancelled appointment with ID {AppointmentId}", id);
                return false;
            }

            if (existing.Status != AppointmentStatus.Cancelled && appointmentDto.Status == AppointmentStatus.Cancelled)
            {
                Log.Information("Marking appointment {AppointmentId} as cancelled", id);
            }

            // Use AutoMapper to map AppointmentDto to Appointment
            var updatedAppointment = _mapper.Map<Appointment>(appointmentDto);
            updatedAppointment.Id = existing.Id; // Keep the existing ID

            _context.Entry(existing).CurrentValues.SetValues(updatedAppointment);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                Log.Warning("Attempted to delete non-existent appointment {AppointmentId}", id);
                return false;
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task TrackMissedAppointmentsAsync(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found");

            if (appointment.Status == AppointmentStatus.Cancelled)
            {
                Log.Information("Skipping missed tracking for cancelled appointment {AppointmentId}", appointmentId);
                return;
            }

            if (appointment.AppointmentDate < DateTimeOffset.UtcNow && appointment.Status == AppointmentStatus.Scheduled)
            {
                appointment.Status = AppointmentStatus.Missed;
                appointment.MissedTimestamp = DateTimeOffset.UtcNow;

                Log.Information("Marked appointment {AppointmentId} as missed", appointmentId);
                await _context.SaveChangesAsync();
            }
        }
    }
}
