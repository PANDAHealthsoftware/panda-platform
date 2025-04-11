using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services.Appointment;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Appointment;
using Serilog;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // POST: api/appointments
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createDto)
    {
        Log.Information(LogMessages.CreatingAppointment, createDto);

        try
        {
            var createdAppointment = await _appointmentService.CreateAsync(createDto);
            Log.Information(LogMessages.AppointmentCreated, createdAppointment.Id);

            return CreatedAtAction(nameof(GetById), new { id = createdAppointment.Id }, createdAppointment);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // GET: api/appointments/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Log.Information(LogMessages.RetrievingAppointment, id);

        try
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
            {
                Log.Warning(LogMessages.AppointmentNotFound, id);
                return NotFound();
            }

            return Ok(appointment);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // GET: api/appointments
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Log.Information(LogMessages.RetrievingAllAppointments);

        try
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // PUT: api/appointments/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        Log.Information(LogMessages.UpdatingAppointment, id, dto);

        try
        {
            var result = await _appointmentService.UpdateAsync(id, dto);
            if (!result)
            {
                Log.Warning(LogMessages.AppointmentNotFound, id);
                return NotFound();
            }

            Log.Information(LogMessages.AppointmentUpdated, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // DELETE: api/appointments/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        Log.Information(LogMessages.CancellingAppointment, id);

        try
        {
            var cancelled = await _appointmentService.DeleteAsync(id);
            if (!cancelled)
            {
                Log.Warning(LogMessages.AppointmentNotFound, id);
                return NotFound();
            }

            Log.Information(LogMessages.AppointmentCancelled, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // POST: api/appointments/track-missed
    [HttpPost("track-missed")]
    public async Task<IActionResult> TrackMissedAppointment([FromQuery] int appointmentId)
    {
        Log.Information(LogMessages.TrackingMissed, appointmentId);

        try
        {
            await _appointmentService.TrackMissedAppointmentsAsync(appointmentId);
            Log.Information(LogMessages.MissedTracked, appointmentId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            Log.Warning(LogMessages.AppointmentNotFound, appointmentId);
            return NotFound();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }
}
