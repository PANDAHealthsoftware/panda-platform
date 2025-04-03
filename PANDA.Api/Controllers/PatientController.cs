using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Dto;
using PANDA.Api.Services;
using PANDA.Api.Common;
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
        Log.Information(LogMessages.RetrievingAllPatients);

        try
        {
            var patients = await _patientService.GetAllPatientsAsync();
            Log.Information(LogMessages.PatientsRetrieved, patients.Count);

            return Ok(patients);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // Create Patient
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePatientDto createDto)
    {
        Log.Information(LogMessages.CreatingPatient, createDto);

        try
        {
            var createdPatient = await _patientService.AddPatientAsync(createDto);
            Log.Information(LogMessages.PatientCreated, createdPatient.Id);

            return CreatedAtAction(nameof(Get), new { id = createdPatient.Id }, createdPatient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // Get Patient by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        Log.Information(LogMessages.RetrievingPatient, id);

        try
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                Log.Warning(LogMessages.PatientNotFound, id);
                return NotFound();
            }

            Log.Information(LogMessages.PatientRetrieved, patient);
            return Ok(patient);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // Update Patient
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientDto patientDto)
    {
        Log.Information(LogMessages.UpdatingPatient, id, patientDto);

        try
        {
            var updatedPatient = await _patientService.UpdatePatientAsync(id, patientDto);
            if (!updatedPatient)
            {
                Log.Warning(LogMessages.PatientNotFound, id);
                return NotFound();
            }

            Log.Information(LogMessages.PatientUpdated, id);
            return NoContent();
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }
}
