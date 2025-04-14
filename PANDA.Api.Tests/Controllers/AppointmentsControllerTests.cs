using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Controllers;
using PANDA.Api.Services.Appointment;
using PANDA.Shared.DTOs.Appointment;
using NSubstitute;

namespace PANDA.Api.Tests.Controllers;

public class AppointmentsControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnAppointmentsInOkResult()
    {
        // Arrange
        var mockService = Substitute.For<IAppointmentService>();
        var expected = new List<AppointmentDto> { new() { Id = 1, PatientName = "John Doe" } };

        mockService.GetAllAsync().Returns(expected);
        var controller = new AppointmentsController(mockService);

        // Act
        var result = await controller.GetAll();

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        okResult.Value.Should().BeEquivalentTo(expected);
    }
}