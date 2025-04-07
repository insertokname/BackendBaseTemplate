using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.RateLimiting;
using BackendBaseTemplate.application.Commands.GenericCommands;
using BackendBaseTemplate.application.Commands.Users;
using BackendBaseTemplate.application.Query.GenericQueries;
using BackendBaseTemplate.application.Query.Users;
using BackendBaseTemplate.domain.Entities;
using BackendBaseTemplate.infrastructure;
using BackendBaseTemplate.infrastructure.Data;
using BackendBaseTemplate.infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.OpenApi.Models;
using BackendBaseTemplate.application.Commands.EntityTemplates;
using BackendBaseTemplate.application.Query.EntityTemplates;

var builder = WebApplication.CreateBuilder(args);
var protocol = builder.Configuration["API_PROTOCOL"]?.ToLower() ?? "http";
if (protocol == "https")
{
    var certPemPath = "/etc/letsencrypt/live/<DOMAIN_NAME>/fullchain.pem";
    var keyPemPath = "/etc/letsencrypt/live/<DOMAIN_NAME>/privkey.pem";

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
        options.ListenAnyIP(8888, listenOptions =>
        {
            listenOptions.UseHttps(x509);
        });
    });
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "jwtToken_Auth_API",
        Version = "v1",
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Jwt token in the format 'Bearer {token}'"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped(typeof(CreateHandler<,>));
builder.Services.AddScoped(typeof(DeleteByIdHandler<>));
builder.Services.AddScoped(typeof(GetAllHandler<>));
builder.Services.AddScoped(typeof(GetRandomHandler<>));

builder.Services.AddScoped(typeof(LoginUserHandler));
builder.Services.AddScoped(typeof(CreateUserHandler));
builder.Services.AddScoped(typeof(SetUserAdminHandler));
builder.Services.AddScoped(typeof(GetUserByIdHandler));

builder.Services.AddScoped(typeof(SetNewTemplateDataObjectHandler));
builder.Services.AddScoped(typeof(GetAllEntityTemplateDataObjectsHandler));

builder.Services.AddSingleton(typeof(TokenProvider));

SecretsManager secretsManager = new SecretsManager(builder.Configuration);
builder.Services.AddSingleton(secretsManager);


builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

                if (userId == secretsManager.DefaultAdminGuid)
                {
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
    });

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("UnauthorizedEndpointRateLimiter", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromSeconds(10)
            }
        )
    );

    options.AddPolicy("LoginRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 6,
                Window = TimeSpan.FromSeconds(60)
            }
        )
    );
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var exceptionHandlerFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature?.Error;

        var responseMessage = app.Environment.IsDevelopment()
            ? exception?.Message
            : "An unexpected error occurred. Please try again later.";

        await context.Response.WriteAsync("{\"error\": \"" + responseMessage + "\"}");
    });
});

//Try to migrate any changes done to the database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    const int maxRetry = 5;
    int retryCount = 0;
    bool migrated = false;

    while (!migrated && retryCount < maxRetry)
    {
        try
        {
            Console.WriteLine("Migrating");
            db.Database.Migrate();
            migrated = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration attempt {retryCount + 1} failed: {ex.Message}");
            retryCount++;
            Thread.Sleep(5000);
        }
    }

    if (!migrated)
    {
        Console.WriteLine("Database migration failed after maximum retries.");
        throw new Exception("Database not ready");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();