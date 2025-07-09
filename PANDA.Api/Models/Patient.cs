using PANDA.Api.Models;
using PANDA.Domain.Enums;
using PANDA.Domain.ValueObjects;

namespace PANDA.Domain.Entities;

public class Patient : AuditableEntity
{
    public int Id { get; private set; }

    public FullName Name { get; private set; } = default!;
    public DateOnly DateOfBirth { get; private set; }
    public Gender? Gender { get; private set; }
    public string NHSNumber { get; private set; } = default!;
    public string Postcode { get; private set; } = default!;

    public int? UserId { get; private set; }
    public User? User { get; private set; }

    // Required by EF Core
    private Patient() { }

    public Patient(FullName name, DateOnly dob, Gender? gender, string nhsNumber, string postcode)
    {
        Name = name;
        DateOfBirth = dob;
        Gender = gender;
        NHSNumber = nhsNumber;
        Postcode = postcode;
    }

    public void UpdateDetails(FullName name, DateOnly dob, Gender? gender, string nhsNumber, string postcode)
    {
        Name = name;
        DateOfBirth = dob;
        Gender = gender;
        NHSNumber = nhsNumber;
        Postcode = postcode;
    }

    public void LinkToUser(int userId)
    {
        UserId = userId;
    }
}