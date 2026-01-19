using System.Text;
using GatewayApi.Extensions;
using GatewayApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

// -------------------------------
// YARP Reverse Proxy
// -------------------------------
builder.Services.AddReverseProxy()
       .LoadFromConfig(configuration.GetSection("ReverseProxy"));

builder.Services.AddJwtAuthentication(configuration);



// -------------------------------
// Configurer l'authentification JWT
// -------------------------------
var jwtSection = configuration.GetSection("Jwt");
var key = Convert.FromBase64String(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {       
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
        
    };
    
});

builder.Services.AddAuthorization();

var app = builder.Build();

// -------------------------------
// Middleware pipeline
// -------------------------------

// Authentification standard ASP.NET Core
app.UseAuthentication();
app.UseAuthorization();

// Custom JWT middleware pour gérer routes publiques et claims
app.UseMiddleware<JwtMiddleware>();

// HTTPS redirection en production
if (environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Reverse Proxy
app.MapReverseProxy();

// Test route
app.MapGet("/", () => "Gateway API running!");

// Lancer l'application
app.Run();

