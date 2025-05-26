using PlatformService;
using PlatformService.SyncDataServices.Grpc;
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
app.MapGrpcService<GrpcPlatformService>();
app.MapGet("/proto/platforms.proto", async context =>
{
    await context.Response.WriteAsync(File.ReadAllText("Protos/platforms.proto"));
});

app.Run();