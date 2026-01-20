using GatewayApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

// -------------------------------
// YARP Reverse Proxy
// -------------------------------
builder.Services.AddReverseProxy()
       .LoadFromConfig(configuration.GetSection("ReverseProxy"));

// -------------------------------
// Authorization standard (pour [Authorize])
// -------------------------------
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AuthenticatedUser", policy =>
        policy.RequireAuthenticatedUser());
});

var app = builder.Build();


// HTTPS redirection en production
if (environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// JWT Middleware : validation + mapping claims
app.UseMiddleware<JwtMiddleware>();

// Authorization pour [Authorize] sur routes protégées
app.UseAuthorization();

// Reverse Proxy
app.MapReverseProxy();

// Test route
app.MapGet("/", () => "Gateway API running!");

// Lancer l'application
app.Run();