using System.Security.Cryptography;
using System.Text;

namespace CalculadoraCedears.Api.Infrastructure.Extensions
{
    public class StringExtensions
    {
        public static string GetRefreshToken()
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            using var sha = SHA256.Create();

            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(refreshToken)));
        }
    }
}
