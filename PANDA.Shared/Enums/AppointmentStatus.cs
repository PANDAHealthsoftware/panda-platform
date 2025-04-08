using System.ComponentModel.DataAnnotations;

namespace PANDA.Shared.Enums;

public enum AppointmentStatus
{
    [Display(Name = "Scheduled")]
    Scheduled = 0,

    [Display(Name = "Attended")]
    Attended = 1,

    [Display(Name = "Missed")]
    Missed = 2,

    [Display(Name = "Cancelled")]
    Cancelled = 3
}