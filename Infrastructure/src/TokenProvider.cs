using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.infrastructure;

public class TokenProvider
{
    private readonly SecretsManager _secretsManager;

    public TokenProvider(SecretsManager secretsManager)
    {
        _secretsManager = secretsManager;

    }
    public string Create(User user)
    {
        string secretKey = _secretsManager.JwtSecret;

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
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_secretsManager.JwtExpirationInMinutes)),
            SigningCredentials = credentials,
            Issuer = _secretsManager.JwtIssuer,
            Audience = _secretsManager.JwtAudience
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}