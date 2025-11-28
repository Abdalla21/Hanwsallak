using Hanwsallak.Domain.Entity;
using Hanwsallak.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hanwsallak.API.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtTokenModel GenerateToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLong!";
            var issuer = jwtSettings["Issuer"] ?? "Hanwsallak";
            var audience = jwtSettings["Audience"] ?? "HanwsallakUsers";
            var tokenExpiryHours = int.Parse(jwtSettings["TokenExpiryHours"] ?? "24");
            var refreshTokenExpiryHours = int.Parse(jwtSettings["RefreshTokenExpiryHours"] ?? "168"); // 7 days

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add roles
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenExpiration = DateTime.UtcNow.AddHours(tokenExpiryHours);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: tokenExpiration,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Generate refresh token
            var refreshToken = Guid.NewGuid().ToString();
            var refreshTokenExpiration = DateTime.UtcNow.AddHours(refreshTokenExpiryHours);

            return new JwtTokenModel
            {
                Token = tokenString,
                TokenExpiryHours = tokenExpiryHours,
                TokenExpiration = tokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiryHours = refreshTokenExpiryHours,
                RefreshTokenExpiration = refreshTokenExpiration,
                TokenType = "Bearer"
            };
        }
    }
}

