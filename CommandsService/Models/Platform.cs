namespace CommandsService.Models;

public class Platform
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    
    public Guid ExternalId { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Command> Commands { get; set; } = [];

}
