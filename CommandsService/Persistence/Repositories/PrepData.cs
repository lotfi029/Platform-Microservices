using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Persistence.Repositories;

public static class PrepData
{
    public static async Task PrepPopulation(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateAsyncScope();
        var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
        var platforms = grpcClient?.ReturnAllPlatforms();
        
        await SeedData(serviceScope.ServiceProvider.GetService<ICommandRespository>()!, platforms!);
    }
    private static async Task SeedData(ICommandRespository commandRespository, IEnumerable<Platform> platforms)
    {
        foreach (var platform in platforms)
        {
            var result = await commandRespository.ExternalPlatformExist(platform.Id);
            if (result.IsFailure)
            {
                var newPlatform = new Platform
                {
                    Id = platform.Id,
                    Name = platform.Name
                };
                await commandRespository.CreatePlatformAsync(newPlatform);
            }
        }
    }
}
