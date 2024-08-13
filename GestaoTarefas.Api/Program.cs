using GestaoTarefas.Domain.Interfaces;
using GestaoTarefas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configuração da connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro de dependências
builder.Services.AddScoped<ITarefaService, TarefaService>();

builder.Services.AddScoped<ITarefaRepository>(provider =>
    new TarefaRepository(connectionString)); // Passando a connection string manualmente

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
