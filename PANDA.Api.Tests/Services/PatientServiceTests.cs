using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Infrastructure;
using PANDA.Api.Models;
using PANDA.Api.Services.Patient;
using PANDA.Shared.Common;
using PANDA.Shared.DTOs.Patient;

namespace PANDA.Api.Tests.Services;
public class PatientServiceTests
{
    private readonly DbContextOptions<PandaDbContext> _options;

    public PatientServiceTests()
    {
        _options = new DbContextOptionsBuilder<PandaDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;
    }

    [Fact]
    public async Task AddPatientAsync_Should_Add_And_Return_Patient()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var dto = new CreatePatientDto()
        {
            FirstName = "Alice",
            LastName = "Smith",
            DateOfBirth = new DateTime(1985, 6, 15),
            NHSNumber = "9876543210",
            Postcode = "AB12 3CD",
            Gender = Gender.Female
        };

        var result = await service.AddPatientAsync(dto);

        result.Id.Should().BeGreaterThan(0);
        result.FirstName.Should().Be("Alice");
        context.Patients.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetPatientByIdAsync_Should_Return_Correct_Patient()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var patient = new Patient
        {
            FirstName = "Bob",
            LastName = "Jones",
            DateOfBirth = DateTime.Today,
            NHSNumber = "1234567890",
            Postcode = "XY99 1YZ",
            Gender = Gender.Male
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var result = await service.GetPatientByIdAsync(patient.Id);
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Bob");
    }

    [Fact]
    public async Task UpdatePatientAsync_Should_Update_And_Return_True()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var patient = new Patient
        {
            FirstName = "Carl",
            LastName = "Dean",
            DateOfBirth = DateTime.Today,
            NHSNumber = "0000000000",
            Postcode = "ZZ1 1ZZ",
            Gender = Gender.Male
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var updateDto = new UpdatePatientDto()
        {
            FirstName = "Charlie",
            LastName = "Dean",
            DateOfBirth = patient.DateOfBirth,
            NHSNumber = patient.NHSNumber,
            Postcode = patient.Postcode,
            Gender = patient.Gender
        };

        var updated = await service.UpdatePatientAsync(patient.Id, updateDto);
        updated.Should().BeTrue();

        var updatedPatient = await context.Patients.FindAsync(patient.Id);
        updatedPatient!.FirstName.Should().Be("Charlie");
    }

    [Fact]
    public async Task UpdatePatientAsync_Should_Return_False_When_Not_Found()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var result = await service.UpdatePatientAsync(999, new UpdatePatientDto()
        {
            FirstName = "Ghost",
            LastName = "User",
            DateOfBirth = DateTime.Today,
            NHSNumber = "0000000000",
            Postcode = "XX1 1XX",
            Gender = Gender.Other
        });

        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeletePatientAsync_Should_Remove_Entity()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var patient = new Patient
        {
            FirstName = "David",
            LastName = "Lee",
            DateOfBirth = DateTime.Today,
            NHSNumber = "1111111111",
            Postcode = "YY1 2YY",
            Gender = Gender.Male
        };

        context.Patients.Add(patient);
        await context.SaveChangesAsync();

        var deleted = await service.DeletePatientAsync(patient.Id);
        deleted.Should().BeTrue();

        var exists = await context.Patients.FindAsync(patient.Id);
        exists.Should().BeNull();
    }

    [Fact]
    public async Task DeletePatientAsync_Should_Return_False_When_Not_Found()
    {
        await using var context = new PandaDbContext(_options);
        var service = new PatientService(context);

        var result = await service.DeletePatientAsync(999);
        result.Should().BeFalse();
    }
}
