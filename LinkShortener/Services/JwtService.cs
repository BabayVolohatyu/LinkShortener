using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace LinkShortener.Services
{
    public class JwtService : IJwtService
    {
        private readonly string secureKey;
        public JwtService(IConfiguration configuration)
        {
            secureKey = configuration["JwtSettings:SecretKey"]
                    ?? throw new ArgumentNullException("Jwt key doesn't exist");
        }
        public string Generate(int? id = null, string? role = null, string? name = null)
        {
            var claims = new List<Claim>();

            if (id.HasValue)
                claims.Add(new Claim("id", id.Value.ToString()));

            if (!string.IsNullOrEmpty(role))
                claims.Add(new Claim(ClaimTypes.Role, role));

            if (!string.IsNullOrEmpty(name))
                claims.Add(new Claim(ClaimTypes.Name, name));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(
                null,
                null,
                claims,
                null,
                DateTime.UtcNow.AddHours(1));

            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
