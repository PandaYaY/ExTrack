namespace Infrastructure.Utils;

public static class UserUtils
{
    public static string PasswordHash(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        return hash;
    }
}
