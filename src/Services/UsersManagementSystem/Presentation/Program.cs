using Application;
using Infrastructure;
using Messaging;

var builder = WebApplication.CreateBuilder(args);
var RabbiMQConfig = builder.Configuration.GetSection("RabbitMQ");

// Register sevices from infrastructure
builder.Services.RegisterInfrastructure(builder.Configuration.GetConnectionString("UserDbCnx")!);

// Register services from application
builder.Services.RegisterApplication();

// Register Messaging services
builder.Services.RegisterMessaging(RabbiMQConfig);

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// addControllers
builder.Services.AddControllers();

// Add swagger generator
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();



