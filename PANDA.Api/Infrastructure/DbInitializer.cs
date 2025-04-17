using Microsoft.EntityFrameworkCore;
using PANDA.Api.Models;
using PANDA.Domain.Entities;
using PANDA.Domain.Enums;
using PANDA.Domain.ValueObjects;
using PANDA.Shared.Enums;

namespace PANDA.Api.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(PandaDbContext context)
    {
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }

        // Seed Clinicians
        var drCarter = new Clinician
        {
            FullName = "Dr. Chris Carter",
            Department = Department.Cardiology,
            Email = "carter@hospital.org",
            PhoneNumber = "01234567890",
            RegistrationNumber = "GMC123456",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var drReid = new Clinician
        {
            FullName = "Dr. Emma Reid",
            Department = Department.Dermatology,
            Email = "reid@hospital.org",
            PhoneNumber = "09876543210",
            RegistrationNumber = "GMC654321",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        if (!context.Clinicians.Any())
        {
            context.Clinicians.AddRange(drCarter, drReid);
            await context.SaveChangesAsync();
        }

        // Seed Users
        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Username = "admin",
                    PasswordHash = "admin123",
                    Role = "Admin"
                },
                new User
                {
                    Username = "reception",
                    PasswordHash = "reception123",
                    Role = "Reception"
                },
                new User
                {
                    Username = "drchris",
                    PasswordHash = "clinician123",
                    Role = "Clinician"
                }
            );
            await context.SaveChangesAsync();
        }

        // Seed Patients
        var john = new Patient(
            new FullName("John", "Doe"),
            new DateOnly(1985, 6, 15),
            Gender.Male,
            "1234567890",
            "AB12 3CD"
        );

        var alice = new Patient(
            new FullName("Alice", "Smith"),
            new DateOnly(1990, 11, 25),
            Gender.Female,
            "9876543210",
            "EF45 6GH"
        );

        if (!context.Patients.Any())
        {
            context.Patients.AddRange(john, alice);
            await context.SaveChangesAsync();
        }

        // Refresh clinician IDs
        drCarter = context.Clinicians.First(c => c.FullName == "Dr. Chris Carter");
        drReid = context.Clinicians.First(c => c.FullName == "Dr. Emma Reid");

        // Seed Appointments
        if (!context.Appointments.Any())
        {
            context.Appointments.AddRange(
                new Appointment
                {
                    PatientId = john.Id,
                    Patient = john,
                    ClinicianId = drCarter.Id,
                    Clinician = drCarter,
                    AppointmentDate = DateTime.Today.AddHours(9),
                    Status = AppointmentStatus.Scheduled,
                    Department = drCarter.Department
                },
                new Appointment
                {
                    PatientId = alice.Id,
                    Patient = alice,
                    ClinicianId = drReid.Id,
                    Clinician = drReid,
                    AppointmentDate = DateTime.Today.AddHours(10),
                    Status = AppointmentStatus.Scheduled,
                    Department = drReid.Department
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
