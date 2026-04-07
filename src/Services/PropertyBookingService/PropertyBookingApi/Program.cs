using Application;
using Infrastructure;
using MediatR; 

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
builder.Services.InjectApplication();
builder.Services.AddInfrastructure(config);

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

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
try
{
    app.MapControllers();
}
catch (System.Reflection.ReflectionTypeLoadException ex)
{
    foreach (var loaderEx in ex.LoaderExceptions)
    {
        Console.WriteLine(loaderEx?.Message);
    }
    throw;
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.Run();

