using PlatformService;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();


builder.Services.AddPlatformServices(builder.Configuration, builder.Environment);
var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
    app.MapOpenApi();
    app.MapScalarApiReference();
//}

app.MapCarter();

app.Run();