using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using BackendOlimpiadaIsto.domain.Entities;

namespace BackendOlimpiadaIsto.infrastructure;

public class TokenProvider
{
    private readonly IConfiguration _configuration;

    public TokenProvider(IConfiguration configuration) {
        _configuration = configuration;
        
    }
    public string Create(User user)
    {
        string? secretKey = _configuration["Jwt:secret"];
        string? envSecretKey = Environment.GetEnvironmentVariable("API_JWT_SECRET");
        string? finalSecret = null;
        if (envSecretKey == null)
        {
            if (secretKey != null)
            {
                finalSecret = secretKey;
                Console.WriteLine("\nONLY APPSETING.JSON API_JWT_SECRET SET!\nTHIS IS ONLY MENT FOR DEVELOPMENT AND IS VERY INSECURE!\nMAKE SURE TO SET THE `API_JWT_SECRET` ENVIRONMENT VARIABLE TO THE ENCODING SECRET WHEN RUNNING THE APP IN PRODUCTION!");
            }
            else
            {
                throw new Exception("No jwt secret key was set!");
            }
        }
        else
        {
            finalSecret = envSecretKey;
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(finalSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            ]),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpirationInMinutes"]!)),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}