using System.Text.Json.Serialization;
using PANDA.Api.Common;
using Panda.Api.Converters;

namespace PANDA.Api.Dto
{
    public class PatientDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime DateOfBirth { get; set; }

        public string NHSNumber { get; set; } = default!;
        public string Postcode { get; set; } = default!;

        public Gender? Gender { get; set; }
        
    }
}