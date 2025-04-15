namespace PANDA.Shared.Security;

public class Roles
{
    public const string Admin = "Admin";
    public const string Clinician = "Clinician";
    public const string Reception = "Reception";

    public const string AdminOrClinician = Admin + "," + Clinician;
    public const string AdminOrReception = Admin + "," + Reception;
}