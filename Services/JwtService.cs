using System.IdentityModel.Tokens.Jwt;
using System.Text;
using SkillSwap.Interfaces;
using SkillSwap.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SkillSwap.Interfaces.Options;

namespace SkillSwap.Services
{
    public interface IJwtService
    {
        public string GenerateToken(ITokenPayload payload);
        public ITokenPayload? VerifyToken(string token);
    }

    public class JwtService : IJwtService
    {
        private readonly IJwtOptions _jwtOptions;
        private readonly JwtSecurityTokenHandler _jwtHandler;
        private readonly SymmetricSecurityKey _securityKey;
        private readonly JwtHeader _jwtHeader;

        public JwtService(IOptions<IJwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _jwtHandler = new JwtSecurityTokenHandler();
            _securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey));
            var credentials = new SigningCredentials(_securityKey, _jwtOptions.EncryptionAlgorithm);
            _jwtHeader = new JwtHeader(credentials);
        }

        public string GenerateToken(ITokenPayload payload)
        {
            var jwtPayload = new JwtPayload() {
                { _jwtOptions.Fields.UserId, Convert.ToInt32(payload.UserId) },
                { _jwtOptions.Fields.Expiration, DateTime.UtcNow.AddSeconds(_jwtOptions.Expiration).Ticks }
            };
            var jwtToken = new JwtSecurityToken(_jwtHeader, jwtPayload);
            var token = _jwtHandler.WriteToken(jwtToken);
            return token;
        }

        public ITokenPayload? VerifyToken(string token)
        {
            try
            {
                _jwtHandler.ValidateToken(token, new TokenValidationParameters()
                {
                    IssuerSigningKey = _securityKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                }, out SecurityToken securityToken);

                var jwtSecurityToken = securityToken as JwtSecurityToken;
                return new()
                {
                    UserId = Convert.ToInt32(jwtSecurityToken!.Payload[_jwtOptions.Fields.UserId])
                };
            }
            catch
            {
                return null;
            }
        }
    }
}