namespace PlatformService.Models;

public class Platform
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Name { get; set; } = string.Empty;
    public string Publisher { get; set; } = string.Empty;
    public string Cost { get; set; } = string.Empty;

}
