using CommandsService.Options;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using PlatformService;
using System.Text.Json;

namespace CommandsService.SyncDataServices.Grpc;

public class PlatformDataClient(IOptions<GrpcPlatformOptions> grpcPlatformOptions) : IPlatformDataClient
{
    private readonly GrpcPlatformOptions _grpcPlatformOptions = grpcPlatformOptions.Value;

    public IEnumerable<Platform> ReturnAllPlatforms()
    {
        Console.WriteLine("Calling gRPC service to get all platforms...");
        var channel = GrpcChannel.ForAddress(_grpcPlatformOptions.Url);
        var client = new GrpcPlatform.GrpcPlatformClient(channel);
        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllPlatform(request);
            
            return reply?.Adapt<IEnumerable<Platform>>() ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return [];
        }
    }
}

