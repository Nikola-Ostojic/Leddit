using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Api.Security
{
    public class JwtHandler
    {
        private readonly JwtSettings _options;

        public JwtHandler(IOptions<JwtSettings> options) =>
            _options = options.Value;

        public string CreateAccessToken(string username, string email, string role)
        {
            var secret = _options.SecretKey;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim("username", username),
            };

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.Now.Add(TimeSpan.FromMinutes(_options.AccessExpiration)),
                signingCredentials: creds)
            {
                Payload = { ["roles"] = new string[] { role } }
            };

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string token, DateTime validFor) CreateRefreshToken() =>
            (
            token: Guid.NewGuid().ToString(),
            validFor: DateTime.Now.Add(TimeSpan.FromMinutes(_options.RefreshExpiration))
            );
    }
}
