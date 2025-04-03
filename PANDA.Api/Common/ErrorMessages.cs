namespace PANDA.Api.Common;

public static class ErrorMessages
{
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";
    public const string AppointmentNotFound = "Appointment with the specified ID was not found.";
    public const string PatientNotFound = "Patient with the specified ID was not found.";
    public const string InvalidAppointmentStatus = "Invalid appointment status.";
    public const string ValidationFailed = "One or more validation errors occurred.";
    public const string AlreadyCancelled = "Cannot update a cancelled appointment.";
    public const string InvalidPatientId = "A valid patient ID is required.";
    public const string FutureAppointmentRequired = "Appointment date must be in the future.";
    public const string ClinicianRequired = "Clinician name is required and must be 100 characters or fewer.";
    public const string InvalidDepartment = "Department must be a valid value.";
}