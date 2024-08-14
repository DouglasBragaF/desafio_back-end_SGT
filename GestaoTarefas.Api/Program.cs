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

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173") // Permite acesso do front-end em localhost:5173
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
