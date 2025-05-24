namespace PlatformService.SyncDataServices.Http;

public interface ICommandDataClient
{
    Task SendPlatformToCommand(PlatformResponse plat);
}
