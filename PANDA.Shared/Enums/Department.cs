namespace PANDA.Shared.Enums;

using System.ComponentModel.DataAnnotations;

public enum Department
{
    [Display(Name = "Cardiology")] Cardiology = 0,

    [Display(Name = "Neurology")] Neurology = 1,

    [Display(Name = "Oncology")] Oncology = 2,

    [Display(Name = "Pediatrics")] Paediatrics = 3,

    [Display(Name = "Dermatology")] Dermatology = 4
}
