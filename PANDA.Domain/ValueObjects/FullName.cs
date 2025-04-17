namespace PANDA.Domain.ValueObjects;

public record FullName(string FirstName, string LastName)
{
    public override string ToString() => $"{FirstName} {LastName}";
}