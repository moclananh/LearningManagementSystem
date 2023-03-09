using System.Security.Cryptography;
using System.Text;


namespace Applications.Utils;

public static class StringUtils
{
    public static string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public static bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
    public static string RandomString()
    {
        var passwordBuilder = new StringBuilder();
        return passwordBuilder.Append(RandomString()).ToString();
    }
}