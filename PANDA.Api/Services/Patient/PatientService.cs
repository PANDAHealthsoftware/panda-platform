using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Patient;
using Serilog;

namespace PANDA.Api.Services.Patient
{
    public class PatientService : IPatientService
    {
        private readonly PandaDbContext _context;

        public PatientService(PandaDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<PatientSummaryDto>> GetPatientSummariesAsync()
        {
            return await _context.Patients
                .Select(p => new PatientSummaryDto
                {
                    Id = p.Id,
                    FullName = p.FirstName + " " + p.LastName
                })
                .ToListAsync();
        }
        
        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _context.Patients.ToListAsync();

            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                NHSNumber = p.NHSNumber,
                Postcode = p.Postcode,
            }).ToList();
        }

        public async Task<Models.Patient> AddPatientAsync(CreatePatientDto patientDto)
        {
            var patient = new Models.Patient
            {
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                DateOfBirth = patientDto.DateOfBirth,
                NHSNumber = patientDto.NHSNumber,
                Postcode = patientDto.Postcode,
                Gender = patientDto.Gender,
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            Log.Information("Patient added with ID: {PatientId}", patient.Id);

            return patient;
        }

        public async Task<Models.Patient?> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<bool> UpdatePatientAsync(int id, PatientDto patientDto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.NHSNumber = patientDto.NHSNumber;
            patient.Postcode = patientDto.Postcode;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePatientAsync(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
