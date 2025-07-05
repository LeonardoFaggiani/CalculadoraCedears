using CalculadoraCedears.Api.Domain;
using CalculadoraCedears.Api.Infrastructure.Extensions;
using CalculadoraCedears.Api.Infrastructure.Repositories;

using CommunityToolkit.Diagnostics;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CalculadoraCedears.Api.CrossCutting.Jwt
{
    public interface IJwtTokenGenerator
    {
        Task<GoogleUserInfo> ValidateAndGenerateTokenAsync(string googleToken);
        Task<GoogleUserInfo> ValidateRefreshTokenAsync(string accessToken, string refreshToken);
    }

    public record GoogleUserInfo(string UserId, string Email, string Jwt, string RefreshToken);
    public record TokenInfo(string Subject, string Name, string Email);

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions options;
        private readonly IGoogleRepository googleRepository;
        private readonly IUserRepository userRepository;

        public JwtTokenGenerator(IOptions<JwtOptions> options,
            IGoogleRepository googleRepository,
            IUserRepository userRepository)
        {
            Guard.IsNotNull(options, nameof(options));
            Guard.IsNotNull(googleRepository, nameof(googleRepository));
            Guard.IsNotNull(userRepository, nameof(userRepository));

            this.options = options.Value;
            this.googleRepository = googleRepository;
            this.userRepository = userRepository;
        }

        public async Task<GoogleUserInfo> ValidateAndGenerateTokenAsync(string googleCode)
        {
            var googleToken = await this.googleRepository.ExchangeCodeAsync(googleCode);

            if (googleToken is null)
                throw new UnauthorizedAccessException("Invalid Google token");

            return new GoogleUserInfo(googleToken.Subject, googleToken.Email, GetAccessToken(googleToken.Subject, googleToken.Name, googleToken.Email), StringExtensions.GetRefreshToken());
        }

        public async Task<GoogleUserInfo> ValidateRefreshTokenAsync(string accessToken, string refreshToken)
        {
            User user = await this.userRepository.All().FirstAsync(x => x.RefreshToken.Equals(refreshToken));

            if (user == null)
                throw new UnauthorizedAccessException("Refresh token inválido");

            if (user.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token expirado");

            TokenInfo tokenInfo = GetTokenInfo(accessToken);

            return new GoogleUserInfo(tokenInfo.Subject, tokenInfo.Email, GetAccessToken(tokenInfo.Subject, tokenInfo.Name, tokenInfo.Email), user.RefreshToken);
        }

        private Claim[] GetClaims(string subject, string name, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, subject),
                new Claim(JwtRegisteredClaimNames.Name, name),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            return claims;
        }

        private string GetAccessToken(string subject, string name, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: options.Issuer,
                audience: options.Audience,
                claims: GetClaims(subject, name, email),
                expires: DateTime.Now.AddMinutes(options.ExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static TokenInfo GetTokenInfo(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var subject = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            var name = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value;

            return new TokenInfo(subject, name, email);
        }
    }
}