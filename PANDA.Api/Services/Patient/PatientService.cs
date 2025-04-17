using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Domain.Entities;
using PANDA.Domain.ValueObjects;
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
                    FullName = p.Name.FirstName + " " + p.Name.LastName
                })
                .ToListAsync();
        }

        public async Task<List<PatientDto>> GetAllPatientsAsync()
        {
            var patients = await _context.Patients.ToListAsync();

            return patients.Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.Name.FirstName,
                LastName = p.Name.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                NHSNumber = p.NHSNumber,
                Postcode = p.Postcode,
            }).ToList();
        }

        public async Task<Domain.Entities.Patient> AddPatientAsync(CreatePatientDto patientDto)
        {
            var fullName = new FullName(patientDto.FirstName, patientDto.LastName);

            var patient = new Domain.Entities.Patient(
                fullName,
                patientDto.DateOfBirth,
                patientDto.Gender,
                patientDto.NHSNumber,
                patientDto.Postcode
            );

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            Log.Information("Patient added with ID: {PatientId}", patient.Id);

            return patient;
        }

        public async Task<Domain.Entities.Patient?> GetPatientByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }

        public async Task<bool> UpdatePatientAsync(int id, UpdatePatientDto patientDto)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return false;

            var fullName = new FullName(patientDto.FirstName, patientDto.LastName);

            patient.UpdateDetails(
                fullName,
                patientDto.DateOfBirth,
                patientDto.Gender,
                patientDto.NHSNumber,
                patientDto.Postcode
            );

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
