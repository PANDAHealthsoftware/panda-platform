namespace PANDA.Shared.Common;

public static class ErrorMessages
{
    // General Errors
    public const string UnexpectedError = "An unexpected error occurred. Please try again later.";

    // Appointment Errors
    public const string AppointmentNotFound = "Appointment with the specified ID was not found.";
    public const string AlreadyCancelled = "Cannot update a cancelled appointment.";
    public const string InvalidAppointmentStatus = "Invalid appointment status.";
    public const string InvalidDepartment = "Department must be a valid value.";
    public const string FutureAppointmentRequired = "Appointment date must be in the future.";
    public const string ClinicianRequired = "Clinician name is required and must be 100 characters or fewer.";
    public const string InvalidPatientId = "A valid patient ID is required.";

    // Patient Errors
    public const string PatientNotFound = "Patient with the specified ID was not found.";
    public const string InvalidGender = "Gender must be a valid value.";
    public const string InvalidNhsNumber = "NHS number must be exactly 10 digits.";
    public const string InvalidNhsChecksum = "NHS number checksum is invalid.";
    public const string NameRequired = "First and last name are required and must be 100 characters or fewer.";
    public const string InvalidDateOfBirth = "Date of birth must be in the past.";
    public const string DateOfBirthRequired = "Date of birth is required.";
    public const string InvalidPostcodeFormat = "Invalid postcode format.";
    
}