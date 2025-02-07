using System.Text;
using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.Users;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.application.Query.PetPrompts;
using BackendOlimpiadaIsto.application.Query.Questions;
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

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o=>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();