using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PANDA.Api.Models;
using PANDA.Shared.Common;
using PANDA.Shared.Enums;

namespace PANDA.Api.Infrastructure;

public class PandaDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    public DbSet<Clinician> Clinicians { get; set; }

    public DbSet<User> Users { get; set; }

    public PandaDbContext(DbContextOptions<PandaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    var dateTimeConverter = new ValueConverter<DateTime, string>(
        v => v.ToString("o"),
        v => DateTime.Parse(v));

    var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
        d => d.ToDateTime(TimeOnly.MinValue),
        d => DateOnly.FromDateTime(d));

    // -----------------------------
    // Global Filters for Soft Delete
    // -----------------------------
    modelBuilder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
    modelBuilder.Entity<Clinician>().HasQueryFilter(c => !c.IsDeleted);
    modelBuilder.Entity<Appointment>().HasQueryFilter(a => !a.IsDeleted);
    modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);

    // -----------------------------
    // Value Conversions
    // -----------------------------
    modelBuilder.Entity<Patient>()
        .Property(p => p.DateOfBirth)
        .HasConversion(dateOnlyConverter);

    modelBuilder.Entity<Appointment>()
        .Property(a => a.AppointmentDate)
        .HasConversion(dateTimeConverter);

    modelBuilder.Entity<Appointment>()
        .Property(a => a.Status)
        .HasConversion<int>();

    // -----------------------------
    // Seed Patients
    // -----------------------------
    modelBuilder.Entity<Patient>().HasData(
        new Patient
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1985, 6, 15),
            Gender = Gender.Male,
            NHSNumber = "1234567890",
            Postcode = "AB12 3CD",
            IsDeleted = false
        },
        new Patient
        {
            Id = 2,
            FirstName = "Alice",
            LastName = "Smith",
            DateOfBirth = new DateOnly(1990, 11, 25),
            Gender = Gender.Female,
            NHSNumber = "9876543210",
            Postcode = "EF45 6GH",
            IsDeleted = false
        }
    );

    // -----------------------------
    // Seed Clinicians
    // -----------------------------
    modelBuilder.Entity<Clinician>().HasData(
        new Clinician
        {
            Id = 1,
            FullName = "Dr. Chris Carter",
            Department = Department.Cardiology,
            IsDeleted = false
        },
        new Clinician
        {
            Id = 2,
            FullName = "Dr. Emma Reid",
            Department = Department.Dermatology,
            IsDeleted = false
        }
    );

    // -----------------------------
    // Seed Appointments
    // -----------------------------
    modelBuilder.Entity<Appointment>().HasData(
        new Appointment
        {
            Id = 1,
            PatientId = 1,
            PatientName = "John Doe",
            AppointmentDate = DateTime.Parse("2025-05-01T10:00:00Z"),
            Status = AppointmentStatus.Scheduled,
            ClinicianId = 1,
            Clinician = "Dr. Chris Carter",
            Department = Department.Cardiology,
            IsDeleted = false
        },
        new Appointment
        {
            Id = 2,
            PatientId = 2,
            PatientName = "Alice Smith",
            AppointmentDate = DateTime.Parse("2025-05-02T14:30:00Z"),
            Status = AppointmentStatus.Scheduled,
            ClinicianId = 2,
            Clinician = "Dr. Emma Reid",
            Department = Department.Dermatology,
            IsDeleted = false
        }
    );

    // -----------------------------
    // Seed Users
    // -----------------------------
    modelBuilder.Entity<User>().HasData(
        new User { Id = 1, Username = "admin", PasswordHash = "admin123", Role = "Admin", IsDeleted = false },
        new User { Id = 2, Username = "reception", PasswordHash = "reception123", Role = "Reception", IsDeleted = false },
        new User { Id = 3, Username = "drchris", PasswordHash = "clinician123", Role = "Clinician", IsDeleted = false }
    );

    base.OnModelCreating(modelBuilder);
}

}