using System.Collections.Generic;
using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PANDA.Shared.DTOs;
using PANDA.Shared.DTOs.Appointment;
using PANDAView.Pages;
using PANDAView.Services.Appointment;
using PANDAView.Services.Patient;
using Radzen;
using Xunit;

namespace PANDAView.Tests.Pages.Appointment;

public class AppointmentsTests : TestContext
{
    [Fact]
    public void ShouldRenderAppointmentList()
    {
        Services.AddScoped<DialogService>();
        Services.AddScoped<NotificationService>();
        Services.AddScoped<TooltipService>();
        Services.AddScoped<ContextMenuService>();

        var mockAppointmentService = new Mock<IAppointmentService>();
        mockAppointmentService.Setup(s => s.GetAppointmentsAsync())
            .ReturnsAsync(new List<AppointmentDto>
            {
                new() { Id = 1, PatientName = "John Doe" }
            });
        Services.AddScoped(_ => mockAppointmentService.Object);

        var mockPatientService = new Mock<IPatientService>();
        mockPatientService.Setup(s => s.GetPatientsAsync())
            .ReturnsAsync(new List<PatientDto>
            {
                new() { Id = 1, FirstName = "Jane", LastName = "Doe" }
            });

        var cut = RenderComponent<Appointments>();

         cut.Markup.Should().Contain("John Doe");
        cut.Markup.Should().Contain("Appointments");

        mockAppointmentService.Verify(s => s.GetAppointmentsAsync(), Times.Once);
        mockPatientService.Verify(s => s.GetPatientsAsync(), Times.Once);
    }
}