namespace CommandsService.Models;

public class Command
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string HowTo { get; set; } = string.Empty;

    public string CommandLine { get; set; } = string.Empty;

    public Guid PlatformId { get; set; }

    public Platform Platform {get; set;} = default!;
}