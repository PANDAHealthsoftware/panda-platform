using System.ComponentModel;

namespace PANDA.Shared.Intents
{
    /// <summary>
    /// Represents the supported intents the PANDA chatbot can classify.
    /// </summary>
    public enum ChatbotIntent
    {
        [Description("User wants to know their appointments today.")]
        GetAppointmentsToday,
        
        [Description("User wants to know their appointments tomorrow.")]
        GetAppointmentsTomorrow,    

        [Description("User wants to know their appointments this week.")]
        GetAppointmentsThisWeek,

        [Description("User wants to know their appointments this month.")]
        GetAppointmentsThisMonth,

        [Description("User wants to know their appointments this year.")]
        GetAppointmentsThisYear,

        [Description("User wants to cancel an appointment.")]
        CancelAppointment,

        [Description("Any other question.")]
        General
    }
}