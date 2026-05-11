using Application;
using Domain.Interfaces;
using Infrastructure;
using MassTransit;
using Presentation;
using Presentation.Helpers;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransitConfigurationExtension(builder.Configuration);

// Register sevices from infrastructure
builder.Services.RegisterInfrastructureExtension(builder.Configuration.GetConnectionString("UserDbCnx")!);

// Register services from application
builder.Services.RegisterApplicationExtension();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Register HttpContextAccessor for CorrelationId
builder.Services.AddScoped<ICorrelationContext, CorrelationContext>();

// addControllers
builder.Services.AddControllers();
// Add swagger generator
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 1. CORRELATION ID (TOUJOURS PREMIER)
app.UseMiddleware<CorrelationIdMiddleware>();

// 2. LOG CONTEXT (Serilog scope)
app.UseMiddleware<LogContextMiddleware>();

// 3. Routing
app.UseRouting();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
