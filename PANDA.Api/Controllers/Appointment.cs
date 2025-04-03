using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Dto;
using PANDA.Api.Models;
using PANDA.Api.Services;
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
        Log.Information("Creating appointment with data: {@AppointmentDto}", createDto);

        try
        {
            var createdAppointment = await _appointmentService.CreateAsync(createDto);
            Log.Information("Appointment created successfully with ID: {AppointmentId}", createdAppointment.Id);

            return CreatedAtAction(nameof(Get), new { id = createdAppointment.Id }, createdAppointment);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating appointment");
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Get Appointment by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Log.Information("Retrieving appointment with ID: {AppointmentId}", id);

        try
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(appointment);
        }
        catch (InvalidOperationException)
        {
            Log.Warning("Appointment with ID {AppointmentId} not found", id);
            return NotFound();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving appointment with ID: {AppointmentId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var appointment = await _appointmentService.GetAllAsync();
            return Ok(appointment);
        }
        catch (InvalidOperationException)
        {
            //Log.Warning("Appointment with ID {AppointmentId} not found", id);
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto appointmentDto)
    {
        Log.Information("Updating appointment with ID {AppointmentId} and data: {@AppointmentDto}", id, appointmentDto);

        try
        {
            var result = await _appointmentService.UpdateAsync(id, appointmentDto);
            if (!result)
            {
                Log.Warning("Appointment with ID {AppointmentId} not found for update", id);
                return NotFound();
            }

            Log.Information("Appointment with ID {AppointmentId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating appointment with ID: {AppointmentId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Cancel Appointment
    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        Log.Information("Cancelling appointment with ID {AppointmentId}", id);

        try
        {
            var cancelled = await _appointmentService.DeleteAsync(id);
            if (!cancelled)
            {
                Log.Warning("Appointment with ID {AppointmentId} not found for cancellation", id);
                return NotFound();
            }

            Log.Information("Appointment with ID {AppointmentId} cancelled successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while cancelling appointment with ID: {AppointmentId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Track missed appointment
    [HttpPost("track-missed-appointment")]
    public async Task<IActionResult> TrackMissedAppointment([FromQuery] int appointmentId)
    {
        Log.Information("Tracking missed appointment for ID: {AppointmentId}", appointmentId);

        try
        {
            await _appointmentService.TrackMissedAppointmentsAsync(appointmentId);
            Log.Information("Tracked missed appointment for ID: {AppointmentId}", appointmentId);
            return Ok();
        }
        catch (KeyNotFoundException)
        {
            Log.Warning("Appointment with ID {AppointmentId} not found for tracking", appointmentId);
            return NotFound();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while tracking missed appointment with ID: {AppointmentId}", appointmentId);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }
}
