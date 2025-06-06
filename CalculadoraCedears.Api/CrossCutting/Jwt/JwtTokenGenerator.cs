using Google.Apis.Auth;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CalculadoraCedears.Api.CrossCutting.Jwt
{
    public interface IJwtTokenGenerator
    {
        Task<string> ValidateAndGenerateTokenAsync(string email, string userId, string googleToken, string googleClientId);
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions options;

        public JwtTokenGenerator(IOptions<JwtOptions> options)
        {
            this.options = options.Value;
        }

        public async Task<string> ValidateAndGenerateTokenAsync(string email, string userId, string googleToken, string googleClientId)
        {
            var validPayload = await ValidateGoogleToken(googleToken, googleClientId);

            if (validPayload is null)
                throw new UnauthorizedAccessException("Invalid Google token");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(options.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string googleToken, string googleClientId)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { googleClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(googleToken, settings);

                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}