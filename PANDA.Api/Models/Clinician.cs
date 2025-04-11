using PANDA.Shared.Enums;

namespace PANDA.Api.Models;

public class Clinician
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public Department Department { get; set; }
}