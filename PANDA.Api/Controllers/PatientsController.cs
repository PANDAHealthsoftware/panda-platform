using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services.Patient;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Patient;
using PANDA.Shared.Security;
using Serilog;
using static PANDA.Shared.Common.LogMessages;

namespace PANDA.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
    public async Task<IActionResult> GetAll()
    {
        Log.Information(messageTemplate: RetrievingAllPatients);
        try
        {
            var patients = await _patientService.GetAllPatientsAsync();
            Log.Information(PatientsRetrieved, patients.Count);
            return Ok(patients);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    [HttpGet("summaries")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
    public async Task<ActionResult<List<PatientSummaryDto>>> GetSummaries()
    {
        Log.Information("Patient summaries endpoint hit by: {User}", User.Identity?.Name);
        return Ok(await _patientService.GetPatientSummariesAsync());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician + "," + Roles.Reception)]
    public async Task<IActionResult> Get(int id)
    {
        Log.Information(RetrievingPatient, id);
        try
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                Log.Warning(PatientNotFound, id);
                return NotFound();
            }

            Log.Information(PatientRetrieved, patient);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician)]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto createDto)
    {
        Log.Information(CreatingPatient, createDto);
        try
        {
            var createdPatient = await _patientService.AddPatientAsync(createDto);
            Log.Information(PatientCreated, createdPatient.Id);
            return CreatedAtAction(nameof(Get), new { id = createdPatient.Id }, createdPatient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin + "," + Roles.Clinician)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto patientDto)
    {
        Log.Information("🔄 Incoming UpdatePatientDto: {@Dto}", patientDto);
        try
        {
            var updated = await _patientService.UpdatePatientAsync(id, patientDto);
            if (!updated)
            {
                Log.Warning("Patient not found for update (ID: {PatientId})", id);
                return NotFound();
            }

            Log.Information("Patient updated (ID: {PatientId})", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error while updating patient");
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }
    
    // DELETE: api/patients/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        Log.Information("Request to delete patient with ID: {PatientId}", id);

        try
        {
            var deleted = await _patientService.DeletePatientAsync(id);
            if (!deleted)
            {
                Log.Warning("Patient not found for deletion (ID: {PatientId})", id);
                return NotFound();
            }

            Log.Information("✅ Patient deleted (ID: {PatientId})", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Unexpected error while deleting patient");
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }
}