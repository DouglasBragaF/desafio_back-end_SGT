using GestaoTarefas.Domain.Interfaces;
using GestaoTarefas.Infrastructure.Repositories;
using GestaoTarefas.Application.Messaging;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using GestaoTarefas.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuração do SignalR
builder.Services.AddSignalR();

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
    x.AddConsumer<TarefaCreatedEventConsumer>();
    x.AddConsumer<TarefaUpdatedEventConsumer>();
    x.AddConsumer<TarefaDeletedEventConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("tarefa-created", e =>
        {
            e.ConfigureConsumer<TarefaCreatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("tarefa-updated", e =>
        {
            e.ConfigureConsumer<TarefaUpdatedEventConsumer>(context);
        });

        cfg.ReceiveEndpoint("tarefa-deleted", e =>
        {
            e.ConfigureConsumer<TarefaDeletedEventConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});
// Configuração da chave, issuer e audience
var secretKey = "your-very-secure-secret-key-32bytes-long";
var issuer = "dev_issuer";
var audience = "dev_audience";

// Instanciando o serviço de geração de tokens JWT
var jwtTokenService = new JwtTokenService(secretKey, issuer, audience);

// Configuração da autenticação JWT
builder.Services.AddSingleton(jwtTokenService);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = jwtTokenService.GetTokenValidationParameters();
    });

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string> ()
        }
    });
});

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

// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<TarefasHub>("/tarefasHub");

app.Run();
