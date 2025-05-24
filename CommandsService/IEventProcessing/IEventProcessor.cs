namespace CommandsService.IEventProcessing;

public interface IEventProcessor
{
    Task ProcessEvent(string messge);
}
