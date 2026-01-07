using Microsoft.AspNetCore.Authentication.JwtBearer;
using Yarp.ReverseProxy;

var builder = WebApplication.CreateBuilder(args);

// Ajouter YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Ajouter JWT Auth pour sécuriser la Gateway
//builder.Services.(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.Authority = "http://localhost:5007"; // AuthService
//        options.RequireHttpsMetadata = false;
//        options.Audience = "gateway_api";
//    });


var app = builder.Build();

//app.UseAuthentication();
//app.UseAuthorization();

// Rediriger toutes les requêtes vers YARP
app.MapReverseProxy();

app.MapGet("/", () => "Hello World!");

app.Run();