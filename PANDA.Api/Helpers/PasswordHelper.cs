using System.Security.Cryptography;
using System.Text;

namespace PANDA.Api.Helpers;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hash);
    }
    
    public static bool VerifyPassword(string password, string storedHash)
    {
        // Basic example using SHA256 (use bcrypt or similar in production!)
        // Hash the input password
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        var hash = Convert.ToBase64String(bytes);

        // Compare hashes
        return hash == storedHash;
    }
}