using System.Security.Cryptography.X509Certificates;

using Infrastructure;

namespace Presentation.Configuration
{
    public static class KestrelConfiguration
    {
        public static void ConfigureKestrel(this WebApplicationBuilder builder, SecretsManager secretsManager)
        {
            var protocol = secretsManager.ApiProtocol;
            if (protocol == "https")
            {
                var certPemPath = $"/etc/letsencrypt/live/{secretsManager.DomainName}/fullchain.pem";
                var keyPemPath = $"/etc/letsencrypt/live/{secretsManager.DomainName}/privkey.pem";

                string fullChain = File.ReadAllText(certPemPath);
                string keyContent = File.ReadAllText(keyPemPath);

                const string certStart = "-----BEGIN CERTIFICATE-----";
                const string certEnd = "-----END CERTIFICATE-----";
                int startIndex = fullChain.IndexOf(certStart);
                int endIndex = fullChain.IndexOf(certEnd);

                if (startIndex < 0 || endIndex < 0)
                {
                    throw new Exception("Certificate PEM not in expected format.");
                }

                int length = endIndex - startIndex + certEnd.Length;
                string leafCert = fullChain.Substring(startIndex, length);

                var x509 = X509Certificate2.CreateFromPem(leafCert, keyContent);

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
                    options.ListenAnyIP(secretsManager.Port, listenOptions => listenOptions.UseHttps(x509));
                });
            }
        }
    }
}
