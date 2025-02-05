using BackendOlimpiadaIsto.application.Query;
using BackendOlimpiadaIsto.infrastructure.Data;
using BackendOlimpiadaIsto.infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<CreateQuestionHandler>();
builder.Services.AddScoped<DeleteQuestionHandler>();
builder.Services.AddScoped<GetAllQuestionsQueryHandler>();
builder.Services.AddScoped<VerifyAnswerQueryHandler>();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();