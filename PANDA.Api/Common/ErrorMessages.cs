namespace PANDA.Api.Common;

public static class ErrorMessages
{
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    public const string AppointmentNotFound = "Appointment with the specified ID was not found.";
    public const string PatientNotFound = "Patient with the specified ID was not found.";
    public const string InvalidAppointmentStatus = "Invalid appointment status.";
    public const string ValidationFailed = "One or more validation errors occurred.";
    public const string AlreadyCancelled = "Cannot update a cancelled appointment.";
}