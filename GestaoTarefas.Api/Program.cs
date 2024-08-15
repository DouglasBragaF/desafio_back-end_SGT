using GestaoTarefas.Domain.Interfaces;
using GestaoTarefas.Infrastructure.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuração da connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Registro de dependências do TarefaService e repositório
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<ITarefaRepository>(provider =>
{
    return new TarefaRepository(connectionString ?? throw new ArgumentNullException("ConnectionString"));
});

// Configuração do MassTransit com RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.WithOrigins("http://localhost:5173")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});

var app = builder.Build();

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
