using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services.Appointment;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.Security;
using Serilog;

namespace PANDA.Api.Controllers;

/// <summary>
/// Controller for handling appointment-related operations.
/// </summary>
/// <remarks>
/// This controller provides endpoints to create, retrieve, update, and cancel appointments,
/// as well as track missed appointments. Access to these endpoints is restricted to users with
/// specific roles such as Admin, Clinician, and Reception.
/// </remarks>
[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    /// <summary>
    /// Represents the service responsible for managing appointment-related operations.
    /// This includes actions such as creating, retrieving, updating, and deleting appointments,
    /// as well as tracking missed appointments.
    /// </summary>
    private readonly IAppointmentService _appointmentService;

    /// <summary>
    /// Provides endpoints for managing appointments within the system. This controller allows
    /// for the creation, retrieval, updating, cancellation, and tracking of missed appointments.
    /// </summary>
    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // POST: api/appointments
    /// Creates a new appointment using the provided appointment details.
    /// <param name="createDto">
    /// The details of the appointment to be created, including patient ID, appointment date, clinician ID, department, status, and reason.
    /// </param>
    /// <returns>
    /// A status indicating the result of the operation. Returns a 201 Created response with the newly created appointment details if successful or a 500 Internal Server Error if an error occurs.
    /// </returns>
    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
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
    /// <summary>
    /// Retrieves an appointment by its unique ID.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the appointment data if found, or a NotFound result if no appointment exists with the provided ID.</returns>
    [HttpGet("{id}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
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
    /// Retrieves a list of all appointments.
    /// This method is accessible to users with roles: Admin, Clinician, or Reception.
    /// It logs the retrieval operation and handles any unexpected errors by returning a 500 status code.
    /// <return>Returns an OkObjectResult containing the list of appointments if successful,
    /// otherwise returns a corresponding error response.</return>
    [HttpGet]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
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
    /// <summary>
    /// Updates the appointment with the specified ID.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to update.</param>
    /// <param name="dto">The data transfer object containing updated appointment details.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        Log.Information(LogMessages.UpdatingAppointment, id, dto);

        try
        {
            var result = await _appointmentService.UpdateAppointmentAsync(id, dto);
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
    /// <summary>
    /// Cancels an appointment by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the appointment to be canceled.</param>
    /// <returns>
    /// Returns a <see cref="NoContentResult"/> if the cancellation is successful.
    /// Returns <see cref="NotFoundResult"/> if the appointment does not exist.
    /// Returns a <see cref="StatusCodeResult"/> with a 500 status code for unexpected errors.
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
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
    /// <summary>
    /// Tracks a missed appointment by its unique identifier, updating the system with relevant information.
    /// </summary>
    /// <param name="appointmentId">The unique identifier of the appointment to be tracked as missed.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns an HTTP 200 response if successful,
    /// 404 if the appointment is not found, or 500 in case of an internal error.</returns>
    [HttpPost("{appointmentId}/track-missed")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician)]
    public async Task<IActionResult> TrackMissedAppointment(int appointmentId)
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
