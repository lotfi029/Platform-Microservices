using CommandsService;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCommandsServices(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();
await PrepData.PrepPopulation(app); // Fix: Added 'await' to ensure the task is awaited.

app.MapControllers();
app.Run();