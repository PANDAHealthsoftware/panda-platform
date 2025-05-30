using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using PANDA.Api.Services.Appointment;
using PANDA.Api.Tests.TestUtilities;
using PANDA.Domain.Entities;
using PANDA.Domain.Enums;
using PANDA.Domain.ValueObjects;
using PANDA.Shared.DTOs.Appointment;
using PANDA.Shared.Enums;

namespace PANDA.Api.Tests.Services;

public class AppointmentServiceTests
{
    private readonly DbContextOptions<PandaDbContext> _options;

    public AppointmentServiceTests()
    {
        _options = new DbContextOptionsBuilder<PandaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;
    }

    [Fact]
    public async Task UpdateAsync_Should_Fail_For_Cancelled_Appointment()
    {
        // Arrange
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, MapperFactory.Create());

        // Seed valid patient
        var patient = new Patient(
            new FullName("Test", "Patient"),
            new DateOnly(1985, 1, 1),
            Gender.Female,
            "9876543210",
            "AB12 3CD"
        );
        context.Patients.Add(patient);

        // Seed valid clinician
        var clinician = new Clinician
        {
            FullName = "Dr. No",
            Department = Department.Cardiology,
            Email = "dr.no@hospital.org",
            PhoneNumber = "0123456789",
            RegistrationNumber = "GMC123456",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Clinicians.Add(clinician);

        await context.SaveChangesAsync();

        // Seed cancelled appointment
        var appointment = new Appointment
        {
            PatientId = patient.Id,
            Patient = patient,
            ClinicianId = clinician.Id,
            Clinician = clinician,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Department = Department.Cardiology,
            Status = AppointmentStatus.Cancelled,
            IsDeleted = false
        };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        // Create update DTO for the cancelled appointment
        var dto = new UpdateAppointmentDto
        {
            PatientId = patient.Id,
            AppointmentDate = DateTime.UtcNow.AddDays(2),
            ClinicianId = 1, // If using ClinicianId now, adjust accordingly
            Department = Department.Neurology,
            Status = AppointmentStatus.Scheduled
        };

        // Act
        Func<Task> act = async () => await service.UpdateAppointmentAsync(appointment.Id, dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Invalid appointment data: patient and clinician are required.");
    }
}
