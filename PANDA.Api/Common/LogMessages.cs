namespace PANDA.Api.Common;

public class LogMessages
{
    //Appointment logs
    public const string CreatingAppointment = "Creating appointment with data: {@AppointmentDto}";
    public const string AppointmentCreated = "Appointment created successfully with ID: {AppointmentId}";

    public const string RetrievingAppointment = "Retrieving appointment with ID: {AppointmentId}";
    public const string AppointmentNotFound = "Appointment with ID {AppointmentId} not found";

    public const string UpdatingAppointment = "Updating appointment with ID {AppointmentId} and data: {@AppointmentDto}";
    public const string AppointmentUpdated = "Appointment with ID {AppointmentId} updated successfully";

    public const string CancellingAppointment = "Cancelling appointment with ID {AppointmentId}";
    public const string AppointmentCancelled = "Appointment with ID {AppointmentId} cancelled successfully";

    public const string TrackingMissed = "Tracking missed appointment for ID: {AppointmentId}";
    public const string MissedTracked = "Tracked missed appointment for ID: {AppointmentId}";

    public const string RetrievingAllAppointments = "Retrieving all appointments";
    
    // Patient logs
    public const string RetrievingAllPatients = "Retrieving all patients";
    public const string PatientsRetrieved = "Retrieved {PatientCount} patients";
    
    public const string CreatingPatient = "Creating patient with data: {@PatientDto}";
    public const string PatientCreated = "Patient created successfully with ID: {PatientId}";

    public const string RetrievingPatient = "Retrieving patient with ID: {PatientId}";
    public const string PatientNotFound = "Patient with ID {PatientId} not found";
    public const string PatientRetrieved = "Patient retrieved successfully: {@Patient}";

    public const string UpdatingPatient = "Updating patient with ID {PatientId} and data: {@PatientDto}";
    public const string PatientUpdated = "Patient with ID {PatientId} updated successfully";
    
    // AppointmentService logs
    public const string UpdateCancelledBlocked = "Attempted to update a cancelled appointment with ID {AppointmentId}";
    public const string AppointmentCancelling = "Marking appointment {AppointmentId} as cancelled";
    public const string AppointmentNotFoundForDelete = "Attempted to delete non-existent appointment {AppointmentId}";
    public const string SkipMissedTrackingCancelled = "Skipping missed tracking for cancelled appointment {AppointmentId}";
    public const string AppointmentMarkedMissed = "Marked appointment {AppointmentId} as missed";
    
}