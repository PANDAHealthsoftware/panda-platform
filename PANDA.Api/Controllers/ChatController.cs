using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PANDA.Api.Services.Appointment;
using PANDA.Api.Services.OpenAi;
using System.Security.Claims;
using PANDA.Shared.Intents;

namespace PANDA.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Require auth so we know the user
public class ChatController : ControllerBase
{
    private readonly IOpenAiService _openAiService;
    private readonly IAppointmentService _appointmentService;

    public ChatController(IOpenAiService openAiService, IAppointmentService appointmentService)
    {
        _openAiService = openAiService;
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string userInput)
    {
        if (string.IsNullOrWhiteSpace(userInput))
            return BadRequest("Message is empty.");

        var intentString = await _openAiService.DetectIntent(userInput);
        if (!Enum.TryParse<ChatbotIntent>(intentString, out var intent))
            intent = ChatbotIntent.General;

        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out var userId) || userId <= 0)
            return Unauthorized("No valid user ID found.");

        // Detect if user is a patient or clinician
        var isPatient = await _appointmentService.IsPatientAsync(userId);
        var isClinician = !isPatient;

        // Route intent
        switch (intent)
        {
            case ChatbotIntent.GetAppointmentsToday:
            {
                var appointments = await _appointmentService.GetAppointmentsForTodayAsync(userId);

                if (appointments == null || !appointments.Any())
                    return Ok("You have no appointments today.");

                var summary = string.Join(", ",
                    appointments.Select(a =>
                        isPatient
                            ? $"{a.AppointmentDate:HH:mm} with {a.ClinicianFullName}"
                            : $"{a.AppointmentDate:HH:mm} with {a.PatientFullName}"));

                return Ok($"You have {appointments.Count} appointment(s) today: {summary}.");
            }

            case ChatbotIntent.GetAppointmentsTomorrow:
            {
                var tomorrow = DateTime.Today.AddDays(1);
                var dayAfterTomorrow = tomorrow.AddDays(1);

                return Ok(await GetAppointmentsInRange(userId, tomorrow, dayAfterTomorrow, "tomorrow", isPatient));
            }

            case ChatbotIntent.GetAppointmentsThisWeek:
            {
                var today = DateTime.Today;
                var end = today.AddDays(7);
                return Ok(await GetAppointmentsInRange(userId, today, end, "this week", isPatient));
            }

            case ChatbotIntent.GetAppointmentsThisMonth:
            {
                var start = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var end = start.AddMonths(1);
                return Ok(await GetAppointmentsInRange(userId, start, end, "this month", isPatient));
            }

            case ChatbotIntent.GetAppointmentsThisYear:
            {
                var start = new DateTime(DateTime.Today.Year, 1, 1);
                var end = start.AddYears(1);
                return Ok(await GetAppointmentsInRange(userId, start, end, "this year", isPatient));
            }

            default:
            {
                var fallback = await _openAiService.GetChatCompletion(userInput);
                return Ok(fallback);
            }
        }

        // Local helper for DRY summary output
        async Task<string> GetAppointmentsInRange(int userId, DateTime start, DateTime end, string label,
            bool isPatient)
        {
            var results = await _appointmentService.GetAppointmentsInRangeAsync(userId, start, end);

            if (results == null || !results.Any())
                return $"You have no appointments {label}.";

            var summary = string.Join(", ",
                results.Select(a =>
                    isPatient
                        ? $"{a.AppointmentDate:dd MMM HH:mm} with {a.ClinicianFullName}"
                        : $"{a.AppointmentDate:dd MMM HH:mm} with {a.PatientFullName}"));

            return $"You have {results.Count} appointment(s) {label}: {summary}.";
        }
    }
}