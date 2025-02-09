using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Users;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.application.Query.PetPrompts;
using BackendOlimpiadaIsto.application.Query.Questions;
using BackendOlimpiadaIsto.domain.Entities;
using BackendOlimpiadaIsto.infrastructure;
using BackendOlimpiadaIsto.infrastructure.Data;
using BackendOlimpiadaIsto.infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped(typeof(CreateCommandHandler<,>));
builder.Services.AddScoped(typeof(DeleteByIdCommandHandler<>));
builder.Services.AddScoped(typeof(GetAllQueryHandler<>));


builder.Services.AddScoped(typeof(VerifyQuestionHandler));
builder.Services.AddScoped(typeof(GetRandomPromptQueryHandler));

builder.Services.AddScoped(typeof(LoginUserCommandHandler));
builder.Services.AddScoped(typeof(CreateUserCommandHandler));

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
                PermitLimit = 5,
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