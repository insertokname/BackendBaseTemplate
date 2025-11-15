using System.Security.Claims;
using System.Text;

using Domain.Entities;

using Infrastructure;
using Infrastructure.Repositories;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Presentation.Configuration
{
    public static class AuthenticationConfiguration
    {
        public static void ConfigureAuthentication(this IServiceCollection services, SecretsManager secretsManager)
        {

            services.AddAuthorization();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretsManager.JwtSecret)),
                        ValidIssuer = secretsManager.JwtIssuer,
                        ValidAudience = secretsManager.JwtAudience,
                        ClockSkew = TimeSpan.Zero
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var userIdClaim = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                            if (!Guid.TryParse(userIdClaim, out var userId))
                            {
                                context.Fail("Invalid user id in token.");
                                return;
                            }

                            var repository = context.HttpContext.RequestServices.GetRequiredService<IRepository<User>>();
                            var user = await repository.GetByIdAsync(userId);
                            if (user == null)
                            {
                                context.Fail("User does not exist.");
                            }
                        }
                    };
                })
                .AddCookie()
                .AddGoogle(options =>
                {
                    options.ClientId = secretsManager.GoogleOauthId;
                    options.ClientSecret = secretsManager.GoogleOauthSecret;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.AccessDeniedPath = "/api/auth/GoogleAccessDenied";
                });

        }
    }
}
