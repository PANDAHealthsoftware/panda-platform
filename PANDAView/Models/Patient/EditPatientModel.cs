using System.ComponentModel.DataAnnotations;
using PANDA.Shared.Common;

namespace PANDAView.Models.Patient;

public class EditPatientModel
{
    public int? Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string NHSNumber { get; set; } = "";
    public DateOnly DateOfBirth { get; set; }
    public string Postcode { get; set; } = "";
    public Gender? Gender { get; set; } 
}