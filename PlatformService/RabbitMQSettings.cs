using System.ComponentModel.DataAnnotations;

namespace PlatformService;

public class RabbitMQSettings
{
    [Required]
    public string Host { get; set; } = string.Empty;
    [Required]
    public int Port { get; set; }
}
