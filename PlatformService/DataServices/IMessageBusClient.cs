namespace PlatformService.DataServices;

public interface IMessageBusClient
{
    Task PublishNewPlatformAsync(PlatformPublishDto platformPublishedDto, CancellationToken ct = default);
}
