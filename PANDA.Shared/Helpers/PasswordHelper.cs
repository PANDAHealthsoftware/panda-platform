namespace PANDA.Shared.Helpers;

public static class PasswordHelper
{
    private const int WorkFactor = 12; // You can adjust this based on your security needs
    
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }
    
    public static bool VerifyPassword(string password, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
    }
    
}