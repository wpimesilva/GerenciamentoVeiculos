using GerenciamentoVeiculos.Data;
using GerenciamentoVeiculos.Messaging;
using GerenciamentoVeiculos.Middleware;
using GerenciamentoVeiculos.Repositories;
using GerenciamentoVeiculos.Repositories.Interfaces;
using GerenciamentoVeiculos.Services;
using GerenciamentoVeiculos.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VehicleOwner.Api.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("VeiculosDb"));

builder.Services.AddScoped<IProprietarioRepository, ProprietarioRepository>();
builder.Services.AddScoped<IVeiculosRepository, VeiculoRepository>();

builder.Services.AddScoped<IProprietarioService, ProprietarioService>();
builder.Services.AddScoped<IVeiculoService, VeiculoService>();

builder.Services.AddSingleton<IMensagemPublisher, AzureServiceBusPublisher>();
builder.Services.AddHostedService<VeiculoMensagemWorker>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Jwt:Authority"];
        options.Audience = builder.Configuration["Jwt:Audience"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
