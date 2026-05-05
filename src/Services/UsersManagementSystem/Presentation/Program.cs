using Application;
using Infrastructure;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
var RabbiMQConfig = builder.Configuration.GetSection("RabbitMQ");

// Add services to the container.

// Register Messaging services
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(RabbiMQConfig["Host"], "/", h =>
        {
            h.Username(RabbiMQConfig["Username"]);
            h.Password(RabbiMQConfig["Password"]);
        });
    });
});

// Register sevices from infrastructure
builder.Services.RegisterInfrastructure(builder.Configuration.GetConnectionString("UserDbCnx")!);

// Register services from application
builder.Services.RegisterApplication();

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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

app.MapControllers();

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();



