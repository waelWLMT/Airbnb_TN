using Application.Interfaces;
using Infrastructure.Clients;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
var userServiceUrl = builder.Configuration["UserManagementService:BaseUrl"];

builder.Services.AddAll(userServiceUrl ?? "");

builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if(app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.Run();
