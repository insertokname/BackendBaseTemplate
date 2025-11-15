using dotenv.net;
using dotenv.net.Utilities;

using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class SecretsManager
    {
        private readonly IConfiguration _configuration;

        public string DbHost { get; }
        public string DbPort { get; }
        public string DbUsername { get; }
        public string DbPassword { get; }
        public string DbName { get; }

        public string DomainName { get; }
        public int Port { get; }
        public string ApiProtocol { get; }

        public string AesSecret { get; }

        public string JwtSecret { get; }
        public string JwtIssuer { get; }
        public string JwtAudience { get; }
        public string JwtExpirationInMinutes { get; }

        public string GoogleOauthId { get; }
        public string GoogleOauthSecret { get; }
        public string GoogleProjectId { get; }
        public string GoogleProjectLocation { get; }
        public string GoogleAiPublisher { get; }
        public string GoogleAiModel { get; }

        private string GetSecret(string secretName)
        {
            EnvReader.TryGetStringValue(secretName, out string? envVarValue);
            if (!string.IsNullOrEmpty(envVarValue))
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

        public void SetSecret(string secretName, string secretValue)
        {
            Environment.SetEnvironmentVariable(secretName, secretValue);
        }

        public SecretsManager(IConfiguration configuration)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var parentDirectory = Path.GetDirectoryName(currentDirectory);
            if (parentDirectory != null)
            {
                try
                {
                    var dotenv = Path.Combine(parentDirectory, ".env");
                    DotEnv.Load(new DotEnvOptions(
                        ignoreExceptions: false,
                        envFilePaths: [dotenv]
                    ));
                }
                catch (System.Exception e)
                {
                    Console.WriteLine($"Errors while reading the .env file!\nGot error: {e}\nThis can be safely ignored if you are not using a .env file");
                }
            }

            _configuration = configuration;
            DbHost = GetSecret("DB_HOST");
            DbPort = GetSecret("DB_PORT");
            DbUsername = GetSecret("DB_USERNAME");
            DbPassword = GetSecret("DB_PASSWORD");
            DbName = GetSecret("DB_NAME");

            AesSecret = GetSecret("AES_SECRET");

            JwtSecret = GetSecret("JWT_SECRET");
            JwtIssuer = GetSecret("JWT_ISSUER");
            JwtAudience = GetSecret("JWT_AUDIENCE");
            JwtExpirationInMinutes = GetSecret("JWT_EXPIRATION_IN_MINUTES");

            DomainName = GetSecret("DOMAIN_NAME");
            Port = int.Parse(GetSecret("PORT"));
            ApiProtocol = GetSecret("API_PROTOCOL");

            GoogleOauthId = GetSecret("GOOGLE_OAUTH_ID");
            GoogleOauthSecret = GetSecret("GOOGLE_OAUTH_SECRET");
            GoogleProjectId = GetSecret("GOOGLE_PROJECT_ID");
            GoogleProjectLocation = GetSecret("GOOGLE_PROJECT_LOCATION");
            GoogleAiPublisher = GetSecret("GOOGLE_PROJECT_AI_PUBLISHER");
            GoogleAiModel = GetSecret("GOOGLE_PROJECT_AI_MODEL");
        }

    }
}
