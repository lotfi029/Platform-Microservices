namespace CommandsService.SyncDataServices.Grpc;

public interface IPlatformDataClient
{
    IEnumerable<Platform> ReturnAllPlatforms();

}

