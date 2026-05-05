using System.Reflection;
using Application;
using AutoMapper;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.AddInfrastructure(config);
builder.Services.InjectApplication();



// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI (remplace AddOpenApi)
builder.Services.AddEndpointsApiExplorer(); // nécessaire pour Minimal APIs
builder.Services.AddSwaggerGen();           // remplace AddOpenApi()


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger middleware
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mon API v1");
        c.RoutePrefix = string.Empty; // Swagger à la racine (https://localhost:5194/index.html)
    });
}

// Mapping des Controllers

app.MapControllers();

if (app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.Run();

