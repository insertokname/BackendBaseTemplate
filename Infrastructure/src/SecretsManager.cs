using Microsoft.Extensions.Configuration;

namespace BackendOlimpiadaIsto.infrastructure;

public class SecretsManager
{
    public readonly IConfiguration _configuration;

    public string DbHost { get; }
    public string DbPort { get; }
    public string DbUsername { get; }
    public string DbPassword { get; }
    public string DbName { get; }

    public string DefaultAdminUsername { get; }
    public string DefaultAdminPassword { get; }

    public string JwtSecret { get; }
    public string JwtIssuer { get; }
    public string JwtAudience { get; }
    public string JwtExpirationInMinutes { get; }

    private string GetSecret(string secretName)
    {
        string? envVarValue = Environment.GetEnvironmentVariable(secretName);
        if (envVarValue != null)
        {
            return envVarValue;
        }
        string? defaultConfigValue = _configuration[$"{secretName}_DEFAULT"];
        if (defaultConfigValue != null)
        {
            Console.WriteLine($"USING DEFAULT VALUE FOR: {secretName} ({secretName}_DEFAULT)\nThis is only ment for development as is very insecure since it is declared in the public git repo!");
            return defaultConfigValue;
        }
        throw new ArgumentException($"Could not find a a environment variable for {secretName} and no default value set inside appsettings.json for {secretName}_DEFAULT!\nPlease make sure one of them is set.");
    }

    public SecretsManager(IConfiguration configuration)
    {
        _configuration = configuration;
        DbHost = GetSecret("DB_HOST");
        DbPort = GetSecret("DB_PORT");
        DbUsername = GetSecret("DB_USERNAME");
        DbPassword = GetSecret("DB_PASSWORD");
        DbName = GetSecret("DB_NAME");

        DefaultAdminUsername = GetSecret("BACKEND_ADMIN_USERNAME");
        DefaultAdminPassword = GetSecret("BACKEND_ADMIN_PASSWORD");

        JwtSecret = GetSecret("JWT_SECRET");
        JwtIssuer = GetSecret("JWT_ISSUER");
        JwtAudience = GetSecret("JWT_AUDIENCE");
        JwtExpirationInMinutes = GetSecret("JWT_EXPIRATION_IN_MINUTES");
    }

}