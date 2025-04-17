using System.Text.Json.Serialization;
using PANDA.Domain.Enums;
using PANDA.Shared.Common;
using PANDA.Shared.Converters;

namespace PANDA.Shared.DTOs
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        
        public DateOnly DateOfBirth { get; set; }

        public string NHSNumber { get; set; } = default!;
        public string Postcode { get; set; } = default!;

        public Gender? Gender { get; set; }
        
        public string FullName => $"{FirstName} {LastName}";
        
    }
}