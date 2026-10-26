using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAppAgroAPI.Configuration;
using WebAppAgroAPI.Data;

namespace WebAppAgroAPI.Service
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }
        public string GenerateToken(User user)
        {
            // 1. claims

            var claims = new List<Claim> 
            {   new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Role, user.UserRole.ToString())
            };

            // 2. create signing key

            byte[] keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            var key = new SymmetricSecurityKey(keyBytes);

            // 3. create signing credentials

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // 4. create Jwt token

            var securityToken = new JwtSecurityToken (
             issuer: _jwtSettings.Issuer,
             audience: _jwtSettings.Audience,
             claims: claims,
             expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
             signingCredentials: credentials
            );

            // 5. convert token to string

            var tokenHandler = new JwtSecurityTokenHandler();

            string token = tokenHandler.WriteToken(securityToken);

            // 6. return token

            return token;
        }
    }
}