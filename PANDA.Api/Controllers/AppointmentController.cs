using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Dto;
using PANDA.Api.Models;
using PANDA.Api.Services;
using PANDA.Api.Common;
using Serilog;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // Create Appointment
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createDto)
    {
        Log.Information(LogMessages.CreatingAppointment, createDto);

        try
        {
            var createdAppointment = await _appointmentService.CreateAsync(createDto);
            Log.Information(LogMessages.AppointmentCreated, createdAppointment.Id);

            return CreatedAtAction(nameof(Get), new { id = createdAppointment.Id }, createdAppointment);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // Get Appointment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
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

    // Get All Appointments
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

    // Update Appointment
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto appointmentDto)
    {
        Log.Information(LogMessages.UpdatingAppointment, id, appointmentDto);

        try
        {
            var result = await _appointmentService.UpdateAsync(id, appointmentDto);
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

    // Cancel Appointment
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

    // Track missed appointment
    [HttpPost("track-missed-appointment")]
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
