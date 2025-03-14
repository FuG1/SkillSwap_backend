using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SkillSwap.Services
{
    public interface IJwtService
    {
        string? GenerateToken(string userId, IDictionary<string, string> claims);
        bool ValidateToken(string token, out IDictionary<string, string> claims);
    }

    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secret = configuration["Jwt:Secret"];
            _issuer = configuration["Jwt:Issuer"];
            _audience = configuration["Jwt:Audience"];
        }

        public string? GenerateToken(string userId, IDictionary<string, string> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            try
            {
                foreach (var claim in claims)
                {
                    tokenClaims.Add(new Claim(claim.Key, claim.Value));
                }

                var token = new JwtSecurityToken(
                    issuer: _issuer,
                    audience: _audience,
                    claims: tokenClaims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch
            {
                return null;
            }
        }

        public bool ValidateToken(string token, out IDictionary<string, string> claims)
        {
            claims = new Dictionary<string, string>();

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                foreach (var claim in principal.Claims)
                {
                    claims[claim.Type] = claim.Value;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}