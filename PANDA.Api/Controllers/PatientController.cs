using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Dto;
using PANDA.Api.Services;
using Serilog;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientController(IPatientService patientService)
    {
        _patientService = patientService;
    }
    // Get All Patients
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Log.Information("Retrieving all patients");

        try
        {
            var patients = await _patientService.GetAllPatientsAsync();
            Log.Information("Retrieved {PatientCount} patients", patients.Count);

            return Ok(patients);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving all patients");
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Create Patient
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto createDto)
    {
        Log.Information("Creating patient with data: {@PatientDto}", createDto);

        try
        {
            var createdPatient = await _patientService.AddPatientAsync(createDto);
            Log.Information("Patient created successfully with ID: {PatientId}", createdPatient.Id);

            return CreatedAtAction(nameof(Get), new { id = createdPatient.Id }, createdPatient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while creating patient");
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Get Patient by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Log.Information("Retrieving patient with ID: {PatientId}", id);

        try
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                Log.Warning("Patient with ID {PatientId} not found", id);
                return NotFound();
            }

            Log.Information("Patient retrieved successfully: {@Patient}", patient);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while retrieving patient with ID: {PatientId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Update Patient
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
    {
        Log.Information("Updating patient with ID {PatientId} and data: {@PatientDto}", id, patientDto);

        try
        {
            var updatedPatient = await _patientService.UpdatePatientAsync(id, patientDto);
            if (!updatedPatient)
            {
                Log.Warning("Patient with ID {PatientId} not found for update", id);
                return NotFound();
            }

            Log.Information("Patient with ID {PatientId} updated successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while updating patient with ID: {PatientId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }

    // Delete Patient
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Log.Information("Deleting patient with ID {PatientId}", id);

        try
        {
            var deleted = await _patientService.DeletePatientAsync(id);
            if (!deleted)
            {
                Log.Warning("Patient with ID {PatientId} not found for deletion", id);
                return NotFound();
            }

            Log.Information("Patient with ID {PatientId} deleted successfully", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while deleting patient with ID: {PatientId}", id);
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }
}