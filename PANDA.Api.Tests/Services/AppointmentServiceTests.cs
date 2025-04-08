using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PANDA.Api.Dto;
using PANDA.Api.Infrastructure;
using PANDA.Api.Mapping;
using PANDA.Api.Models;
using PANDA.Api.Services;
using PANDA.Shared.DTOs;
using PANDA.Shared.Enums;

namespace PANDA.Api.Tests.Services;

public class AppointmentServiceTests
{
    private readonly DbContextOptions<PandaDbContext> _options;
    private readonly IMapper _mapper;

    public AppointmentServiceTests()
    {
        _options = new DbContextOptionsBuilder<PandaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateAsync_Should_Add_And_Return_AppointmentDto()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var dto = new CreateAppointmentDto
        {
            PatientId = 1,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. Who",
            Department = "Cardiology"
        };

        var result = await service.CreateAsync(dto);

        result.Id.Should().BeGreaterThan(0);
        result.PatientId.Should().Be(1);
        result.Clinician.Should().Be("Dr. Who");
        result.Department.Should().Be(Department.Cardiology);
        context.Appointments.Count().Should().Be(1);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_AppointmentDto()
    {
        await using var context = new PandaDbContext(_options);
        var appointment = new Appointment
        {
            PatientId = 2,
            AppointmentDate = DateTime.UtcNow.AddDays(2),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. Strange",
            Department = Department.Cardiology
        };
        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var service = new AppointmentService(context, _mapper);
        var result = await service.GetByIdAsync(appointment.Id);

        result.Should().NotBeNull();
        result!.PatientId.Should().Be(2);
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_And_Return_True()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var appointment = new Appointment
        {
            PatientId = 3,
            AppointmentDate = DateTime.UtcNow.AddDays(3),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. House",
            Department = Department.Cardiology
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var updated = new UpdateAppointmentDto()
        {
            PatientId = 3,
            AppointmentDate = DateTime.UtcNow.AddDays(4),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. House",
            Department = Department.Cardiology
        };

        var result = await service.UpdateAsync(appointment.Id, updated);
        result.Should().BeTrue();

        var updatedEntry = await context.Appointments.FindAsync(appointment.Id);
        updatedEntry.Should().NotBeNull();
        updatedEntry!.AppointmentDate.Should().BeCloseTo(updated.AppointmentDate, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task DeleteAsync_Should_Mark_Appointment_As_Cancelled()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var appointment = new Appointment
        {
            PatientId = 4,
            AppointmentDate = DateTime.UtcNow.AddDays(5),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. McCoy",
            Department = Department.Cardiology
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var result = await service.DeleteAsync(appointment.Id);
        result.Should().BeTrue();

        var exists = await context.Appointments.FindAsync(appointment.Id);
        exists.Should().NotBeNull();
        exists!.Status.Should().Be(AppointmentStatus.Cancelled);
    }

    [Fact]
    public async Task TrackMissedAppointmentsAsync_Should_Mark_As_Missed()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var appointment = new Appointment
        {
            PatientId = 5,
            AppointmentDate = DateTime.UtcNow.AddDays(-1),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. Smith",
            Department = Department.Cardiology
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        await service.TrackMissedAppointmentsAsync(appointment.Id);

        var updated = await context.Appointments.FindAsync(appointment.Id);
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(AppointmentStatus.Missed);
    }

    [Fact]
    public async Task TrackMissedAppointmentsAsync_Should_Not_Override_Cancelled()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var appointment = new Appointment
        {
            PatientId = 7,
            AppointmentDate = DateTime.UtcNow.AddDays(-2),
            Status = AppointmentStatus.Cancelled,
            Clinician = "Dr. Banner",
            Department = Department.Neurology
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        await service.TrackMissedAppointmentsAsync(appointment.Id);

        var updated = await context.Appointments.FindAsync(appointment.Id);
        updated.Should().NotBeNull();
        updated!.Status.Should().Be(AppointmentStatus.Cancelled);
    }

    [Fact]
    public async Task UpdateAsync_Should_Fail_For_Cancelled_Appointment()
    {
        await using var context = new PandaDbContext(_options);
        var service = new AppointmentService(context, _mapper);

        var appointment = new Appointment
        {
            PatientId = 6,
            AppointmentDate = DateTime.UtcNow.AddDays(1),
            Status = AppointmentStatus.Cancelled,
            Clinician = "Dr. Watson",
            Department = Department.Neurology
        };

        context.Appointments.Add(appointment);
        await context.SaveChangesAsync();

        var updated = new UpdateAppointmentDto
        {
            PatientId = 6,
            AppointmentDate = DateTime.UtcNow.AddDays(2),
            Status = AppointmentStatus.Scheduled,
            Clinician = "Dr. Watson",
            Department = Department.Paediatrics
        };

        var result = await service.UpdateAsync(appointment.Id, updated);
        result.Should().BeFalse();
    }
}
