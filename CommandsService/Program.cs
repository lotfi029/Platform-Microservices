using CommandsService;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCommandsServices(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapControllers();

app.Run();