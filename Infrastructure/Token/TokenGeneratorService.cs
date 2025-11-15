using System.Security.Claims;
using System.Text;

using Domain.Entities;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Token
{
    public class TokenGeneratorService(
        SecretsManager secretsManager)
    {
        public string Create(User user)
        {
            string secretKey = secretsManager.JwtSecret;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                     user.IsAdmin ?
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "Admin")
                    ]
                    :
                    [new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),]
                ),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(secretsManager.JwtExpirationInMinutes)),
                SigningCredentials = credentials,
                Issuer = secretsManager.JwtIssuer,
                Audience = secretsManager.JwtAudience
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
