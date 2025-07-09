using Microsoft.EntityFrameworkCore;
using PANDA.Api.Helpers;
using PANDA.Api.Models;
using PANDA.Domain.Entities;
using PANDA.Domain.Enums;
using PANDA.Domain.ValueObjects;
using PANDA.Shared.Enums;
using PANDA.Shared.Helpers;

namespace PANDA.Api.Infrastructure;

public static class DbInitializer
{
    public static async Task SeedAsync(PandaDbContext context)
    {
        // ---------------------------------------------
        // 1️⃣ Apply pending migrations
        // ---------------------------------------------
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }

        // ---------------------------------------------
        // 2️⃣ Clear all data in FK order
        // ---------------------------------------------
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Appointments");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Patients");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Clinicians");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Users");
        await context.Database.ExecuteSqlRawAsync("DELETE FROM Roles");

        // ---------------------------------------------
// 3️⃣ Seed Roles with fixed IDs using a single transaction
// ---------------------------------------------
        using (var transaction = await context.Database.BeginTransactionAsync())
        {
            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Roles ON");

            context.Roles.AddRange(
                new Role { Id = (int)RoleType.Admin, Name = "Admin" },
                new Role { Id = (int)RoleType.Reception, Name = "Reception" },
                new Role { Id = (int)RoleType.Clinician, Name = "Clinician" },
                new Role { Id = (int)RoleType.Patient, Name = "Patient" }
            );
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Roles OFF");

            await transaction.CommitAsync();
        }

        await context.SaveChangesAsync();
        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Roles OFF");

        // ---------------------------------------------
        // 4️⃣ Seed Users and link RoleId
        // ---------------------------------------------
        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = PasswordHelper.HashPassword("admin123"),
            RoleId = (int)RoleType.Admin,
            Status = "Active",
            IsStaff = true
        };

        var receptionUser = new User
        {
            Username = "reception",
            PasswordHash = PasswordHelper.HashPassword("reception123"),
            RoleId = (int)RoleType.Reception,
            Status = "Active",
            IsStaff = true
        };

        var drChrisUser = new User
        {
            Username = "drchris",
            PasswordHash = PasswordHelper.HashPassword("clinician123"),
            RoleId = (int)RoleType.Clinician,
            Status = "Active",
            IsStaff = true
        };

        var johnUser = new User
        {
            Username = "johndoe",
            PasswordHash = PasswordHelper.HashPassword("patient123"),
            RoleId = (int)RoleType.Patient,
            Status = "Active",
            IsStaff = false
        };

        var aliceUser = new User
        {
            Username = "alicesmith",
            PasswordHash = PasswordHelper.HashPassword("patient123"),
            RoleId = (int)RoleType.Patient,
            Status = "Active",
            IsStaff = false
        };

        context.Users.AddRange(adminUser, receptionUser, drChrisUser, johnUser, aliceUser);
        await context.SaveChangesAsync();

        // ---------------------------------------------
        // 5️⃣ Seed Clinicians — link to drChrisUser
        // ---------------------------------------------
        var drCarter = new Clinician
        {
            FullName = "Dr. Chris Carter",
            Department = Department.Cardiology,
            Email = "carter@hospital.org",
            PhoneNumber = "01234567890",
            RegistrationNumber = "GMC123456",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UserId = drChrisUser.Id
        };

        var drReid = new Clinician
        {
            FullName = "Dr. Emma Reid",
            Department = Department.Dermatology,
            Email = "reid@hospital.org",
            PhoneNumber = "09876543210",
            RegistrationNumber = "GMC654321",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UserId = null // no user yet for drReid
        };

        context.Clinicians.AddRange(drCarter, drReid);
        await context.SaveChangesAsync();

        // ---------------------------------------------
        // 6️⃣ Seed Patients — link to johnUser & aliceUser
        // ---------------------------------------------
        var john = new Patient(
            new FullName("John", "Doe"),
            new DateOnly(1985, 6, 15),
            Gender.Male,
            "1234567890",
            "AB12 3CD"
        );
        john.LinkToUser(johnUser.Id);

        var alice = new Patient(
            new FullName("Alice", "Smith"),
            new DateOnly(1990, 11, 25),
            Gender.Female,
            "9876543210",
            "EF45 6GH"
        );
        alice.LinkToUser(aliceUser.Id);

        context.Patients.AddRange(john, alice);
        await context.SaveChangesAsync();

        // 7️⃣ Seed Appointments for today
        context.Appointments.AddRange(
            new Appointment
            {
                PatientId = john.Id,
                Patient = john,
                ClinicianId = drCarter.Id, // ✅ FIXED
                Clinician = drCarter,
                AppointmentDate = DateTime.Today.AddHours(9),
                Status = AppointmentStatus.Scheduled,
                Department = drCarter.Department
            },
            new Appointment
            {
                PatientId = alice.Id,
                Patient = alice,
                ClinicianId = drReid.Id, // ✅ FIXED
                Clinician = drReid,
                AppointmentDate = DateTime.Today.AddHours(10),
                Status = AppointmentStatus.Scheduled,
                Department = drReid.Department
            }
        );
        await context.SaveChangesAsync();
    }
}