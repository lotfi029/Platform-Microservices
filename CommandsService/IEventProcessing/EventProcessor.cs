using System.Text.Json;

namespace CommandsService.IEventProcessing;

public class EventProcessor(IServiceScopeFactory _serviceScopeFactory) : IEventProcessor
{
    public async Task ProcessEvent(string messge)
    {
        var eventType = await Task.Run(() => DetermineEventType(messge));

        switch (eventType)
        {
            case EventType.PlatformPublished:
                await AddPlatform(messge);
                break;
            default:
                break;
        }
    }
    private static EventType DetermineEventType(string message)
    {
        var dMsg = JsonSerializer.Deserialize<GenericEventDto>(message);

        switch (dMsg?.Event)
        {
            case "Platform_Published":
                Console.WriteLine("Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("Could not determine the event type");
                return EventType.UnDetermined;
        }
    }
    private async Task AddPlatform(string message)
    {
        using var scop = _serviceScopeFactory.CreateScope();
        var _repo = scop.ServiceProvider.GetRequiredService<ICommandRespository>();
        try
        {
            var platform = message.Adapt<Platform>();
            var exist = await _repo.ExternalPlatformExist(platform.ExternalId);

            if (!exist.IsSuccess)
            {
                await _repo.CreatePlatformAsync(platform);
            }
            else
            {
                Console.WriteLine("Platform already exists");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add platform to DB: {ex.Message}");
        }
    }
}

enum EventType
{
    PlatformPublished,
    UnDetermined
}