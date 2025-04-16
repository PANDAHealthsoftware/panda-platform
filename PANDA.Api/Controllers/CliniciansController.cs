using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services;
using PANDA.Api.Services.Clinician;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Clinician;
using Serilog;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")] // Resolves to: api/clinicians
public class CliniciansController : ControllerBase
{
    private readonly IClinicianService _clinicianService;

    public CliniciansController(IClinicianService clinicianService)
    {
        _clinicianService = clinicianService;
    }

    // POST: api/clinicians
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClinicianDto createDto)
    {
        Log.Information("Creating clinician {@Clinician}", createDto);

        try
        {
            var created = await _clinicianService.CreateAsync(createDto);
            Log.Information("Clinician created with ID {Id}", created.Id);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // GET: api/clinicians/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Log.Information("Retrieving clinician with ID {Id}", id);

        try
        {
            var clinician = await _clinicianService.GetByIdAsync(id);
            if (clinician == null)
            {
                Log.Warning("Clinician not found with ID {Id}", id);
                return NotFound();
            }

            return Ok(clinician);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }

    // GET: api/clinicians
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Log.Information("Retrieving all clinicians");

        try
        {
            var clinicians = await _clinicianService.GetAllAsync();
            return Ok(clinicians);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ErrorMessages.UnexpectedError);
            return StatusCode(500, ErrorMessages.UnexpectedError);
        }
    }
}
