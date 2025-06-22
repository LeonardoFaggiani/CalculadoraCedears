using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CalculadoraCedears.Api.CrossCutting.Jwt
{
    public interface IJwtTokenGenerator
    {
        Task<GoogleUserInfo> ValidateAndGenerateTokenAsync(string googleToken);
    }

    public record GoogleUserInfo(string UserId, string Email, string Jwt);
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions options;
        private readonly IGoogleRepository googleRepository;

        public JwtTokenGenerator(IOptions<JwtOptions> options, IGoogleRepository googleRepository)
        {
            Guard.IsNotNull(options, nameof(options));
            Guard.IsNotNull(googleRepository, nameof(googleRepository));

            this.options = options.Value;
            this.googleRepository = googleRepository;
        }

        public async Task<GoogleUserInfo> ValidateAndGenerateTokenAsync(string googleCode)
        {
            var googleToken = await this.googleRepository.ExchangeCodeAsync(googleCode);

            if (googleToken is null)
                throw new UnauthorizedAccessException("Invalid Google token");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, googleToken.Subject),
                new Claim(JwtRegisteredClaimNames.Name, googleToken.Name),
                new Claim(JwtRegisteredClaimNames.Email, googleToken.Email),
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

            return new GoogleUserInfo(googleToken.Subject, googleToken.Email, new JwtSecurityTokenHandler().WriteToken(token));
        }   
    }
}